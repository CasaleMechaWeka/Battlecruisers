using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
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

            InitialiseFactory(_leftFactory, Faction.Reds, Direction.Right, leftUnit);
            InitialiseFactory(_rightFactory, Faction.Blues, Direction.Left, rightUnit);


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

        private void InitialiseFactory(IFactory factory, Faction faction, Direction facingDirection, IBuildableWrapper<IUnit> unitWrapper)
        {
            _helper
                .InitialiseBuilding(
                    factory,
                    faction,
                    parentCruiserDirection: facingDirection);

            factory.CompletedBuildable += (sender, e) =>
			{
                factory.UnitWrapper = unitWrapper;
                factory.CompletedBuildingUnit += Factory_CompletedUnit;
			};

            factory.Destroyed += (sender, e) => OnScenarioComplete();

            factory.StartConstruction();
        }

        private float FindUnitCost(IPrefabKey unitKey)
        {
            IBuildableWrapper<IUnit> unit = _prefabFactory.GetUnitWrapperPrefab(unitKey);
            return unit.Buildable.NumOfDronesRequired * unit.Buildable.BuildTimeInS;
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
