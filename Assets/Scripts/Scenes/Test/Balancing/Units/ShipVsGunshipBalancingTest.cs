using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class ShipVsGunshipBalancingTest : MonoBehaviour, ITestScenario
    {
        private IFactory _navalFactory, _airFactory;
        private IKillCountController _aircraftKillCount, _shipsKillCount;
        private IList<ITarget> _completedUnits;

        private const int GUNSHIP_CRUISING_ALTITUDE_IN_M = 10;
        private const int GUNSHIP_PATROLLING_RANGE_IN_M = 20;

        protected TestUtils.Helper _helper;
        protected IPrefabFactory _prefabFactory;

        public int numOfDrones;
        public PrefabKeyName shipPrefabKeyName;

        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);
            Assert.IsTrue(numOfDrones > 0);


            _prefabFactory = prefabFactory;
            _helper = new TestUtils.Helper(numOfDrones, BuildSpeedMultipliers.DEFAULT);
            _completedUnits = new List<ITarget>();

            IPrefabKey gunshipKey = StaticPrefabKeys.Units.Gunship;
            IPrefabKey shipKey = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(shipPrefabKeyName);

            ShowScenarioDetails(shipKey, gunshipKey);

            IBuildableWrapper<IUnit> ship = _prefabFactory.GetUnitWrapperPrefab(shipKey);
            IBuildableWrapper<IUnit> gunship = _prefabFactory.GetUnitWrapperPrefab(gunshipKey);

            _aircraftKillCount = InitialiseKillCount("ShipsKillCount", gunship.Buildable);
            _shipsKillCount = InitialiseKillCount("AircraftKillCount", ship.Buildable);


            // Initlialise factories
            _navalFactory = GetComponentInChildren<NavalFactory>();
            _airFactory = GetComponentInChildren<AirFactory>();

            InitialiseFactory(_navalFactory, Faction.Reds, Direction.Right, ship, _shipsKillCount);
            InitialiseFactory(_airFactory, Faction.Blues, Direction.Left, gunship, _aircraftKillCount);


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;
        }

        private void ShowScenarioDetails(IPrefabKey shipKey, IPrefabKey gunshipKey)
        {
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = "Drones: " + numOfDrones + "  " + shipKey.PrefabPath.GetFileName() + " vs " + gunshipKey.PrefabPath.GetFileName();
        }

        private IKillCountController InitialiseKillCount(string componentName, IBuildable enemyBuildable)
        {
            KillCountController killCount = transform.FindNamedComponent<KillCountController>(componentName);
            killCount.Initialise((int)enemyBuildable.CostInDroneS);
            return killCount;
        }

        private void InitialiseFactory(
            IFactory factory,
            Faction faction,
            Direction facingDirection,
            IBuildableWrapper<IUnit> unitWrapper,
            IKillCountController killCounter)
        {
            IList<Vector2> gunshipPatrolPoints = GetGunshipPatrolPoints(factory.Position, GUNSHIP_CRUISING_ALTITUDE_IN_M);
            IAircraftProvider aircraftProvider = _helper.CreateAircraftProvider(gunshipPatrolPoints: gunshipPatrolPoints);

            _helper
                .InitialiseBuilding(
                    factory,
                    faction,
                    parentCruiserDirection: facingDirection,
                    aircraftProvider: aircraftProvider);

            factory.CompletedBuildable += (sender, e) => OnFactoryCompleted(factory, unitWrapper, killCounter);
            factory.Destroyed += (sender, e) => OnScenarioComplete();

            factory.StartConstruction();
        }

        private IList<Vector2> GetGunshipPatrolPoints(Vector2 factoryPosition, float cruisingAltitudeInM)
        {
            return new List<Vector2>()
            {
                new Vector2(factoryPosition.x, cruisingAltitudeInM),
                new Vector2(factoryPosition.x - GUNSHIP_PATROLLING_RANGE_IN_M, cruisingAltitudeInM)
            };
        } 

        private void OnFactoryCompleted(IFactory factory, IBuildableWrapper<IUnit> unitToBuild, IKillCountController killCounter)
        {
            factory.StartBuildingUnit(unitToBuild);
            factory.CompletedBuildingUnit += (sender, e) => OnFactoryCompletedUnit(e.CompletedUnit, killCounter);
        }

        private void OnFactoryCompletedUnit(IBuildable completedUnit, IKillCountController killCounter)
        {
            _completedUnits.Add(completedUnit);
            completedUnit.Destroyed += (sender, e) => killCounter.KillCount++;
        }

        protected void OnScenarioComplete()
        {
            // Stop producing units
            if (!_navalFactory.IsDestroyed)
            {
                _navalFactory.StopBuildingUnit();
            }
            if (!_airFactory.IsDestroyed)
            {
                _airFactory.StopBuildingUnit();
            }

            int currentAircraftKillCount = _aircraftKillCount.KillCount;
            int currentShipKillCount = _shipsKillCount.KillCount;

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
            _aircraftKillCount.KillCount = currentAircraftKillCount;
            _shipsKillCount.KillCount = currentShipKillCount;
        }
    }
}
