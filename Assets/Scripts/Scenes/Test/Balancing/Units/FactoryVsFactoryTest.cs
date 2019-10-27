using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class FactoryVsFactoryTest : MonoBehaviour, ITestScenario
    {
        private IKillCountController _leftKillCount, _rightKillCount;
        private IList<ITarget> _completedUnits;

		protected IFactory _leftFactory, _rightFactory;
        protected TestUtils.Helper _helper;
        protected IDeferrer _deferrer;

        public int numOfDrones;
        public PrefabKeyName leftUnitKeyName, rightUnitKeyName;

        public Camera Camera { get; private set; }

        public void Initialise(TestUtils.Helper parentHelper)
        {
            Assert.IsNotNull(parentHelper);
            Assert.IsTrue(numOfDrones > 0);

            _deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(_deferrer);

            _helper
                = new TestUtils.Helper(
                    parentHelper,
                    numOfDrones,
                    BuildSpeedMultipliers.DEFAULT);
            _completedUnits = new List<ITarget>();

            IPrefabKey leftUnitKey = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(leftUnitKeyName);
            IPrefabKey rightUnitKey = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(rightUnitKeyName);

            ShowScenarioDetails(leftUnitKey, rightUnitKey);

            IBuildableWrapper<IUnit> leftUnit = parentHelper.PrefabFactory.GetUnitWrapperPrefab(leftUnitKey);
            IBuildableWrapper<IUnit> rightUnit = parentHelper.PrefabFactory.GetUnitWrapperPrefab(rightUnitKey);

            _leftKillCount = InitialiseKillCount("LeftFactoryKillCount", rightUnit.Buildable);
            _rightKillCount = InitialiseKillCount("RightFactoryKillCount", leftUnit.Buildable);


            // Initlialise factories
            IFactory[] factories = GetComponentsInChildren<IFactory>(includeInactive: true);
            Assert.IsTrue(factories.Length == 2);
            factories = factories.OrderBy(factory => factory.Position.x).ToArray();

            _leftFactory = factories[0];
            _rightFactory = factories[1];

            float leftFactoryWaitTime = FindFactoryWaitTimeInS(leftUnit.Buildable, rightUnit.Buildable);
            float rightFactoryWaitTime = FindFactoryWaitTimeInS(rightUnit.Buildable, leftUnit.Buildable);

            ICruiser blueCruiser = _helper.CreateCruiser(Direction.Left, Faction.Blues);
            ICruiser redCruiser = _helper.CreateCruiser(Direction.Right, Faction.Reds);

            InitialiseFactory(_leftFactory, Faction.Reds, Direction.Right, leftUnit, leftFactoryWaitTime, _rightKillCount, redCruiser, blueCruiser, _helper.UpdaterProvider);
            InitialiseFactory(_rightFactory, Faction.Blues, Direction.Left, rightUnit, rightFactoryWaitTime, _leftKillCount, blueCruiser, redCruiser, _helper.UpdaterProvider);


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;
        }

        private void ShowScenarioDetails(IPrefabKey leftUnitKey, IPrefabKey rightUnitKey)
        {
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = "Drones: " + numOfDrones + "  " + leftUnitKey.PrefabPath.GetFileName() + " vs " + rightUnitKey.PrefabPath.GetFileName();
        }

        private IKillCountController InitialiseKillCount(string componentName, IBuildable enemyBuildable)
        {
            KillCountController killCount = transform.FindNamedComponent<KillCountController>(componentName);
            killCount.Initialise((int)enemyBuildable.CostInDroneS);
            return killCount;
        }

        /// <summary>
        /// We want the first unit of each factory to be completed at the same time.
        /// That is to prevent a cheaper unit from completing quickly, and destroying
        /// the more expensive unit before it is even finished!  Hence, delay the
        /// cheaper unit's factory.
        /// </summary>
        private float FindFactoryWaitTimeInS(IBuildable ownUnit, IBuildable otherUnit)
        {
            if (ownUnit.CostInDroneS >= otherUnit.CostInDroneS)
            {
                // More expensive unit => No wait time
                return 0;
            }
            else
            {
                // Cheaper unit => Have some wait time
                float waitTimeInDroneS = otherUnit.CostInDroneS - ownUnit.CostInDroneS;
                return waitTimeInDroneS / numOfDrones / BuildSpeedMultipliers.DEFAULT;
            }
        }

        private void InitialiseFactory(
            IFactory factory,
            Faction faction,
            Direction facingDirection,
            IBuildableWrapper<IUnit> unitWrapper,
            float waitTimeInS, 
            IKillCountController killCounter,
            ICruiser parentCruiser, 
            ICruiser enemyCruiser,
            IUpdaterProvider updaterProvider)
        {
            IAircraftProvider aircraftProvider = CreateAircraftProvider(facingDirection);
            TestUtils.BuildableInitialisationArgs args
                = new TestUtils.BuildableInitialisationArgs(
                    _helper,
                    faction,
                    parentCruiserDirection: facingDirection,
                    updaterProvider: updaterProvider,
                    parentCruiser: parentCruiser,
                    enemyCruiser: enemyCruiser,
                    aircraftProvider: aircraftProvider,
                    deferrer: _deferrer);
            _helper.InitialiseBuilding(factory, args);

            factory.CompletedBuildable += (sender, e) => OnFactoryCompleted(factory, unitWrapper, waitTimeInS, killCounter);
            factory.Destroyed += (sender, e) => OnScenarioComplete();

            factory.GameObject.SetActive(true);
            factory.StartConstruction();
            TestUtils.Helper.SetupFactoryForUnitMonitor(factory, parentCruiser);
        }

        protected virtual IAircraftProvider CreateAircraftProvider(Direction facingDirection)
        {
            return null;
        }

        private void OnFactoryCompleted(IFactory factory, IBuildableWrapper<IUnit> unitToBuild, float waitTimeInS, IKillCountController killCounter)
        {
            _deferrer.Defer(() => factory.StartBuildingUnit(unitToBuild), waitTimeInS);

            factory.UnitCompleted += (sender, e) => OnFactoryCompletedUnit(e.CompletedUnit, killCounter);
        }

        private void OnFactoryCompletedUnit(IBuildable completedUnit, IKillCountController killCounter)
        {
            _completedUnits.Add(completedUnit);
            completedUnit.Destroyed += (sender, e) => killCounter.KillCount++;
        }

        protected void OnScenarioComplete()
        {
            // Stop producing units
            if (!_leftFactory.IsDestroyed)
            {
                _leftFactory.StopBuildingUnit();
			}
            if (!_rightFactory.IsDestroyed)
            {
                _rightFactory.StopBuildingUnit();
			}

            int currentLeftKillCount = _leftKillCount.KillCount;
            int currentRightKillCount = _rightKillCount.KillCount;

            // Destroy all units (because behaviour is undefined they have no more
            // targets, means the game is won).
            foreach (ITarget target in _completedUnits)
            {
                if (!target.IsDestroyed)
                {
                    target.Destroy();
                }
            }

            // Do NOT count units destroyed programmatically at scenario end
            // towards the kill count.
            _leftKillCount.KillCount = currentLeftKillCount;
            _rightKillCount.KillCount = currentRightKillCount;
        }
    }
}
