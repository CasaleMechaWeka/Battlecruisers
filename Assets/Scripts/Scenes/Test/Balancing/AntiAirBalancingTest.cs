using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class AntiAirBalancingTest : MonoBehaviour, ITargetConsumer
    {
        private TestUtils.Helper _helper;
        private IPrefabFactory _prefabFactory;
        private IList<ITarget> _antiAirBuildings;
        private int _numOfAntiAirBuildings;
        private IPrefabKey _bomberKey;
        private IFactory _airFactory;
        private IList<ITarget> _completedBombers;

        public int numOfBomberDrones;
        public int numOfAntiAirTurrets;
        public int numOfSamSites;

        private const int ANTI_AIR_BUILDINGS_OFFSET_IN_M = 15;
        private const int ANTI_AIR_BUILDINGS_GAP_IN_M = 2;
        private const int BOMBER_CRUISING_ALTITUDE_IN_M = 15;

        public Camera Camera { get; private set; }

        public ITarget Target
        {
            set
            {
                if (value == null)
                {
                    // Bombers have destroyed all targets :D
					
                    // Stop producing bombers
					_airFactory.UnitWrapper = null;

                    // Destroy all bombers (because they cannot handle not 
                    // having targets, means the game is won)
                    foreach (ITarget target in _completedBombers)
                    {
                        if (!target.IsDestroyed)
                        {
                            target.Destroy();
                        }
                    }
                }
            }
        }

        public void Initialise(IPrefabFactory prefabFactory, IPrefabKey bomberKey, IPrefabKey antiAirKey, IPrefabKey samSiteKey)
        {
            Assert.IsTrue(numOfBomberDrones > 0);
            Assert.IsTrue(numOfAntiAirTurrets >= 0);
            Assert.IsTrue(numOfSamSites >= 0);


            _bomberKey = bomberKey;
            _numOfAntiAirBuildings = numOfAntiAirTurrets + numOfSamSites;
            _helper = new TestUtils.Helper(numOfDrones: numOfBomberDrones);
            _prefabFactory = prefabFactory;
            _antiAirBuildings = new List<ITarget>(_numOfAntiAirBuildings);
            _completedBombers = new List<ITarget>();


            // Show test case details
            TextMesh detailsText = GetComponentInChildren<TextMesh>();
            detailsText.text = "Drones: " + numOfBomberDrones + "  AA: " + numOfAntiAirTurrets + "  SS: " + numOfSamSites;


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;


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
                building.CompletedBuildable += Building_CompletedBuildable;
                building.StartConstruction();
            }

            return currentOffsetInM;
        }

        private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            ITarget completedBuilding = sender.Parse<ITarget>();
            _antiAirBuildings.Add(completedBuilding);

            if (_antiAirBuildings.Count == _numOfAntiAirBuildings)
            {
                _airFactory = CreateAirFactory();
            }
        }

        private IFactory CreateAirFactory()
        {
            AirFactory factory = GetComponentInChildren<AirFactory>();
            IList<Vector2> bomberPatrolPoints = GetBomberPatrolPoints(factory.transform.position, BOMBER_CRUISING_ALTITUDE_IN_M);
            IAircraftProvider aircraftProvider = _helper.CreateAircraftProvider(bomberPatrolPoints);
            ITargetsFactory targetsFactory = _helper.CreateBomberTargetsFactory(_antiAirBuildings);

            // So we know when (if) the bombers manage to destroy all targets
            targetsFactory.BomberTargetProcessor.AddTargetConsumer(this);
            targetsFactory.BomberTargetProcessor.StartProcessingTargets();

            _helper
                .InitialiseBuilding(
                    factory, 
                    Faction.Blues, 
                    parentCruiserDirection: Direction.Right, 
                    aircraftProvider: aircraftProvider,
                    targetsFactory: targetsFactory);

            factory.CompletedBuildable += (sender, eventArgs) =>
            {
                factory.UnitWrapper = _prefabFactory.GetUnitWrapperPrefab(_bomberKey);
                factory.CompletedBuildingUnit += (s, e) => _completedBombers.Add(e.Buildable);
            };
            factory.StartConstruction();

            return factory;
        }

        private IList<Vector2> GetBomberPatrolPoints(Vector2 factoryPosition, float bomberCruisingAltitudeInM)
        {
            return new List<Vector2>()
            {
                new Vector2(factoryPosition.x, bomberCruisingAltitudeInM),
                new Vector2(ANTI_AIR_BUILDINGS_OFFSET_IN_M, bomberCruisingAltitudeInM)
            };
        }    
    }
}
