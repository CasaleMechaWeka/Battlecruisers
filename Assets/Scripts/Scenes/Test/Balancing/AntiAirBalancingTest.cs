using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
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

        private const int ANTI_AIR_BUILDINGS_OFFSET_IN_M = 10;
        private const int ANTI_AIR_BUILDINGS_GAP_IN_M = 2;

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
            _helper.InitialiseBuilding(factory, Faction.Blues, parentCruiserDirection: Direction.Right);
            factory.CompletedBuildable += (sender, e) => 
            {
                ((Factory)sender).UnitWrapper = _prefabFactory.GetUnitWrapperPrefab(bomberKey);
            };
            factory.StartConstruction();


            // Create anti air buildings
            int currentOffsetInM = CreateBuildings(antiAirKey, numOfAntiAirTurrets, ANTI_AIR_BUILDINGS_OFFSET_IN_M);
            CreateBuildings(samSiteKey, numOfSamSites, currentOffsetInM);
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

