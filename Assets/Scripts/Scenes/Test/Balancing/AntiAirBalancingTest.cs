using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class AntiAirBalancingTest : MonoBehaviour
    {
        private Helper _helper;
        private IPrefabFactory _prefabFactory;

        public int numOfBomberDrones;
        public int numOfAntiAirTurrets;
        public int numOfSamSites;

        private const int ANTI_AIR_BUILDINGS_OFFSET_IN_M = 15;
        private const int ANTI_AIR_BUILDINGS_GAP_IN_M = 2;
        private const int BOMBER_CRUISING_ALTITUDE_IN_M = 15;

        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory, IPrefabKey bomberKey, IPrefabKey antiAirKey, IPrefabKey samSiteKey)
        {
            Assert.IsTrue(numOfBomberDrones > 0);
            Assert.IsTrue(numOfAntiAirTurrets >= 0);
            Assert.IsTrue(numOfSamSites >= 0);


            _helper = new Helper(numOfDrones: numOfBomberDrones);
            _prefabFactory = prefabFactory;


            // Show test case details
            TextMesh detailsText = GetComponentInChildren<TextMesh>();
            detailsText.text = "Drones: " + numOfBomberDrones + "  AA: " + numOfAntiAirTurrets + "  SS: " + numOfSamSites;


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;


            // Initialise air factory
            AirFactory factory = GetComponentInChildren<AirFactory>();
            IList<Vector2> bomberPatrolPoints = GetBomberPatrolPoints(factory.transform.position, BOMBER_CRUISING_ALTITUDE_IN_M);
            IAircraftProvider aircraftProvider = _helper.CreateAircraftProvider(bomberPatrolPoints);
            _helper.InitialiseBuilding(factory, Faction.Blues, parentCruiserDirection: Direction.Right, aircraftProvider: aircraftProvider);

            factory.CompletedBuildable += (sender, e) => 
            {
                ((Factory)sender).UnitWrapper = _prefabFactory.GetUnitWrapperPrefab(bomberKey);
            };
            factory.StartConstruction();


            // Create anti air buildings
            int currentOffsetInM = CreateBuildings(antiAirKey, numOfAntiAirTurrets, ANTI_AIR_BUILDINGS_OFFSET_IN_M);
            CreateBuildings(samSiteKey, numOfSamSites, currentOffsetInM);
        }

        private IList<Vector2> GetBomberPatrolPoints(Vector2 factoryPosition, float bomberCruisingAltitudeInM)
        {
            return new List<Vector2>()
            {
                new Vector2(factoryPosition.x, bomberCruisingAltitudeInM),
                new Vector2(ANTI_AIR_BUILDINGS_OFFSET_IN_M, bomberCruisingAltitudeInM)
            };
        }

        /// <returns>Cumulative building offset.</returns>
        private int CreateBuildings(IPrefabKey buildingKey, int numOfBuildings, int currentOffsetInM)
        {
            for (int i = 0; i < numOfBuildings; ++i)
            {
                IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(buildingKey);
                IBuilding building = _prefabFactory.CreateBuilding(buildingWrapper);
                _helper.InitialiseBuilding(building, Faction.Reds);

                building.Position = new Vector2(currentOffsetInM, 0);
                currentOffsetInM += ANTI_AIR_BUILDINGS_GAP_IN_M;
                building.StartConstruction();
            }

            return currentOffsetInM;
        }
    }
}

