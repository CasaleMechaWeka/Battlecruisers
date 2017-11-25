using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.UIWrappers;
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
        private ITextMesh _bomberKillCountText, _bomberCostText;
        private float _bomberCostInDroneS;

        public int numOfBomberDrones;
        public int numOfAntiAirTurrets;
        public int numOfSamSites;

        private const int ANTI_AIR_BUILDINGS_OFFSET_IN_M = 15;
        private const int ANTI_AIR_BUILDINGS_GAP_IN_M = 2;
        private const int BOMBER_CRUISING_ALTITUDE_IN_M = 15;
        private const string BOMBER_KILL_COUNT_PREFIX = "Dead bombers: ";
        private const string BOMBER_COST_PREFIX = "Bomber cost (drone seconds): ";
        private const string ANTI_AIR_BUILDINGS_COST_PREFIX = "Building cost (drone secodns): ";

        public Camera Camera { get; private set; }

        private int _bomberKillCount;
        private int BomberKillCount
        {
            get { return _bomberKillCount; }
            set
            {
                _bomberKillCount = value;
                _bomberKillCountText.Text = BOMBER_KILL_COUNT_PREFIX + _bomberKillCount;
                _bomberCostText.Text = BOMBER_COST_PREFIX + (_bomberKillCount * _bomberCostInDroneS);
            }
        }

        public ITarget Target
        {
            set
            {
                if (value == null)
                {
                    // Bombers have destroyed all targets :D
					
                    // Stop producing bombers
					_airFactory.UnitWrapper = null;

                    int currentBomberKillCount = BomberKillCount;

                    // Destroy all bombers (because they cannot handle not 
                    // having targets, means the game is won)
                    foreach (ITarget target in _completedBombers)
                    {
                        if (!target.IsDestroyed)
                        {
                            target.Destroy();
                        }
                    }

                    // Do NOT count bombers destroyed programmatically at scenario end
                    // towards the kill count.
                    BomberKillCount = currentBomberKillCount;
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
            _bomberCostInDroneS = FindUnitCost(_bomberKey);


            SetupTexts(antiAirKey, samSiteKey);


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;


            // Create anti air buildings
            int originalOffsetInM = (int)transform.position.x + ANTI_AIR_BUILDINGS_OFFSET_IN_M;
            int currentOffsetInM = CreateBuildings(antiAirKey, numOfAntiAirTurrets, originalOffsetInM);
            CreateBuildings(samSiteKey, numOfSamSites, currentOffsetInM);
        }

        private void SetupTexts(IPrefabKey antiAirKey, IPrefabKey samSiteKey)
        {
            // Show test case details
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = "Drones: " + numOfBomberDrones + "  AA: " + numOfAntiAirTurrets + "  SS: " + numOfSamSites;

            // Anti air building cost
            TextMesh buildingCostText = transform.FindNamedComponent<TextMesh>("BuildingDroneSecSpentText");
            int buildingCost
                = FindBuildingCost(antiAirKey, numOfAntiAirTurrets)
                + FindBuildingCost(samSiteKey, numOfSamSites);
            buildingCostText.text = ANTI_AIR_BUILDINGS_COST_PREFIX + buildingCost;

            // Bomber cost
            TextMesh bomberCostText = transform.FindNamedComponent<TextMesh>("BomberDroneSecSpentText");
            _bomberCostText = new TextMeshWrapper(bomberCostText);

			// Bomber kill count
			TextMesh bomberKillCountText = transform.FindNamedComponent<TextMesh>("BomberKillCountText");
            _bomberKillCountText = new TextMeshWrapper(bomberKillCountText);
			BomberKillCount = 0;
        }

        private int FindBuildingCost(IPrefabKey buildingKey, int numOfBuildings)
        {
            IBuildableWrapper<IBuilding> building = _prefabFactory.GetBuildingWrapperPrefab(buildingKey);
            return building.Buildable.NumOfDronesRequired * (int)building.Buildable.BuildTimeInS * numOfBuildings;
        }

        private float FindUnitCost(IPrefabKey unitKey)
        {
            IBuildableWrapper<IUnit> unit = _prefabFactory.GetUnitWrapperPrefab(unitKey);
            return unit.Buildable.NumOfDronesRequired * unit.Buildable.BuildTimeInS;
        }

        /// <returns>Cumulative building offset.</returns>
        private int CreateBuildings(IPrefabKey buildingKey, int numOfBuildings, int currentOffsetInM)
        {
            for (int i = 0; i < numOfBuildings; ++i)
            {
                IBuildableWrapper<IBuilding> buildingWrapper = _prefabFactory.GetBuildingWrapperPrefab(buildingKey);
                IBuilding building = _prefabFactory.CreateBuilding(buildingWrapper);
                _helper.InitialiseBuilding(building, Faction.Reds, parentCruiserDirection: Direction.Left);

                building.Position = new Vector2(currentOffsetInM, 0);

                // Mirror building, so it is facing left
                building.Rotation = Helper.MirrorAccrossYAxis(building.Rotation);

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
                factory.CompletedBuildingUnit += Factory_CompletedBomber;
            };
            factory.StartConstruction();

            return factory;
        }

        private void Factory_CompletedBomber(object sender, CompletedConstructionEventArgs e)
        {
            _completedBombers.Add(e.Buildable);
            e.Buildable.Destroyed += Bomber_Destroyed;
        }

        private void Bomber_Destroyed(object sender, DestroyedEventArgs e)
        {
            BomberKillCount++;
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
