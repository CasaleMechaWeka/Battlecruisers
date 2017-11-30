using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class ShipVsShipBalancingTest : MonoBehaviour, ITestScenario
    {
        private IFactory _leftFactory, _rightFactory;
        private IKillCountController _leftKillCount, _rightKillCount;
		private IList<ITarget> _completedShips;
        private VariableDelayDeferrer _deferrer;

        protected TestUtils.Helper _helper;
        protected IPrefabFactory _prefabFactory;

        public int numOfDrones;

        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory, IPrefabKey leftShipKey, IPrefabKey rightShipKey)
        {
            Helper.AssertIsNotNull(prefabFactory, leftShipKey, rightShipKey);
            Assert.IsTrue(numOfDrones > 0);


			_prefabFactory = prefabFactory;
            _helper = new TestUtils.Helper(numOfDrones: numOfDrones);
            _completedShips = new List<ITarget>();

            _deferrer = GetComponent<VariableDelayDeferrer>();
            Assert.IsNotNull(_deferrer);

            ShowScenarioDetails(leftShipKey, rightShipKey);

            IBuildableWrapper<IUnit> leftUnit = _prefabFactory.GetUnitWrapperPrefab(leftShipKey);
            IBuildableWrapper<IUnit> rightUnit = _prefabFactory.GetUnitWrapperPrefab(rightShipKey);

            _leftKillCount = InitialiseKillCount("LeftShipsKillCount", leftUnit.Buildable);
            _rightKillCount = InitialiseKillCount("RightShipsKillCount", rightUnit.Buildable);


            // Initlialise factories
            IFactory[] factories = GetComponentsInChildren<IFactory>();
            Assert.IsTrue(factories.Length == 2);
            factories = factories.OrderBy(factory => factory.Position.x).ToArray();

            _leftFactory = factories[0];
            _rightFactory = factories[1];

            float leftFactoryWaitTime = FindFactoryWaitTimeInS(leftUnit.Buildable, rightUnit.Buildable);
            float rightFactoryWaitTime = FindFactoryWaitTimeInS(rightUnit.Buildable, leftUnit.Buildable);

            InitialiseFactory(_leftFactory, Faction.Reds, Direction.Right, leftUnit, leftFactoryWaitTime);
            InitialiseFactory(_rightFactory, Faction.Blues, Direction.Left, rightUnit, rightFactoryWaitTime);


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;
        }

        private void ShowScenarioDetails(IPrefabKey leftShipKey, IPrefabKey rightShipKey)
        {
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = "Drones: " + numOfDrones + "  " + leftShipKey.PrefabPath.GetFileName() + " vs " + rightShipKey.PrefabPath.GetFileName();
        }

        private IKillCountController InitialiseKillCount(string componentName, IBuildable buildable)
        {
            KillCountController killCount = transform.FindNamedComponent<KillCountController>(componentName);
            killCount.Initialise((int)buildable.CostInDroneS);
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
                return waitTimeInDroneS / numOfDrones / Buildable.BUILD_CHEAT_MULTIPLIER;
            }
        }

        private void InitialiseFactory(
            IFactory factory,
            Faction faction,
            Direction facingDirection,
            IBuildableWrapper<IUnit> unitWrapper,
            float waitTimeInS)
        {
            _helper
                .InitialiseBuilding(
                    factory,
                    faction,
                    parentCruiserDirection: facingDirection);

            factory.CompletedBuildable += (sender, e) => OnFactoryCompleted(factory, unitWrapper, waitTimeInS);
            factory.Destroyed += (sender, e) => OnScenarioComplete();

            factory.StartConstruction();
        }

        private void OnFactoryCompleted(IFactory factory, IBuildableWrapper<IUnit> unitToBuild, float waitTimeInS)
        {
            _deferrer.Defer(() => factory.UnitWrapper = unitToBuild, waitTimeInS);

            // FELIX  Use method, pevent reference check in method below?
			factory.CompletedBuildingUnit += Factory_CompletedUnit;
        }

        private void Factory_CompletedUnit(object sender, CompletedConstructionEventArgs e)
        {
            IFactory factory = sender.Parse<IFactory>();

            _completedShips.Add(e.Buildable);

            e.Buildable.Destroyed += (destroyedShip, eventArgs) => 
            {
                // If left factory produced unit that was killed, right side gets the kill
                if (ReferenceEquals(factory, _leftFactory))
                {
                    _rightKillCount.KillCount++;
                }
                else
                {
                    _leftKillCount.KillCount++;
                }
            };
        }

        protected void OnScenarioComplete()
        {
            // Stop producing units
            if (!_leftFactory.IsDestroyed)
            {
                _leftFactory.UnitWrapper = null;
			}
            if (!_rightFactory.IsDestroyed)
            {
                _rightFactory.UnitWrapper = null;
			}

            int currentLeftKillCount = _leftKillCount.KillCount;
            int currentRightKillCount = _rightKillCount.KillCount;

            // Destroy all units (because behaviour is undefined they have no more
            // targets, means the game is won).
            foreach (ITarget target in _completedShips)
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
