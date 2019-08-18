using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.BattleScene.Update;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class FactoryVsFactoryTest : MonoBehaviour, ITestScenario
    {
        private IKillCountController _leftKillCount, _rightKillCount;
        private IList<ITarget> _completedUnits;
        private IPrefabFactory _prefabFactory;

		protected IFactory _leftFactory, _rightFactory;
        protected TestUtils.Helper _helper;
        protected IDeferrer _deferrer;

        public int numOfDrones;
        public PrefabKeyName leftUnitKeyName, rightUnitKeyName;

        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory, IUpdaterProvider updaterProvider)
        {
            Assert.IsNotNull(prefabFactory);
            Assert.IsTrue(numOfDrones > 0);

            _deferrer = new Deferrer();
            _prefabFactory = prefabFactory;
            _helper = new TestUtils.Helper(numOfDrones, BuildSpeedMultipliers.DEFAULT, updaterProvider: updaterProvider);
            _completedUnits = new List<ITarget>();

            IPrefabKey leftUnitKey = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(leftUnitKeyName);
            IPrefabKey rightUnitKey = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(rightUnitKeyName);

            ShowScenarioDetails(leftUnitKey, rightUnitKey);

            IBuildableWrapper<IUnit> leftUnit = _prefabFactory.GetUnitWrapperPrefab(leftUnitKey);
            IBuildableWrapper<IUnit> rightUnit = _prefabFactory.GetUnitWrapperPrefab(rightUnitKey);

            _leftKillCount = InitialiseKillCount("LeftFactoryKillCount", rightUnit.Buildable);
            _rightKillCount = InitialiseKillCount("RightFactoryKillCount", leftUnit.Buildable);


            // Initlialise factories
            IFactory[] factories = GetComponentsInChildren<IFactory>();
            Assert.IsTrue(factories.Length == 2);
            factories = factories.OrderBy(factory => factory.Position.x).ToArray();

            _leftFactory = factories[0];
            _rightFactory = factories[1];

            float leftFactoryWaitTime = FindFactoryWaitTimeInS(leftUnit.Buildable, rightUnit.Buildable);
            float rightFactoryWaitTime = FindFactoryWaitTimeInS(rightUnit.Buildable, leftUnit.Buildable);

            ICruiser blueCruiser = _helper.CreateCruiser(Direction.Left, Faction.Blues);
            ICruiser redCruiser = _helper.CreateCruiser(Direction.Right, Faction.Reds);

            InitialiseFactory(_leftFactory, Faction.Reds, Direction.Right, leftUnit, leftFactoryWaitTime, _rightKillCount, redCruiser, blueCruiser, updaterProvider);
            InitialiseFactory(_rightFactory, Faction.Blues, Direction.Left, rightUnit, rightFactoryWaitTime, _leftKillCount, blueCruiser, redCruiser, updaterProvider);


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
            TestUtils.BuildableInitialisationArgs args = CreateFactoryArgs(faction, facingDirection, parentCruiser, enemyCruiser, updaterProvider);
            _helper.InitialiseBuilding(factory, args);

            factory.CompletedBuildable += (sender, e) => OnFactoryCompleted(factory, unitWrapper, waitTimeInS, killCounter);
            factory.Destroyed += (sender, e) => OnScenarioComplete();

            factory.StartConstruction();
            TestUtils.Helper.SetupFactoryForUnitMonitor(factory, parentCruiser);
        }

        protected virtual TestUtils.BuildableInitialisationArgs CreateFactoryArgs(
            Faction faction, 
            Direction facingDirection,
            ICruiser parentCruiser, 
            ICruiser enemyCruiser, 
            IUpdaterProvider updaterProvider)
        {
            return new TestUtils.BuildableInitialisationArgs(_helper, faction, parentCruiserDirection: facingDirection, updaterProvider: updaterProvider);
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
