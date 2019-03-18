using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Scenes.Test.Balancing.Spawners;
using BattleCruisers.Scenes.Test.Balancing.Units;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Defensives
{
    public abstract class DefenceBuildingBalancingTest : MonoBehaviour, ITestScenario
    {
        private int _numOfDefenceBuildings;
        private IPrefabKey _offensiveUnitKey;
        private IFactory _offensiveFactory;
        private IList<ITarget> _completedOffensiveUnits;
        private IKillCountController _unitKillCount;
		private IList<ITarget> _defenceBuildings;
        private int _numOfDefenceBuildngsDestroyed;

        protected TestUtils.Helper _helper;
        protected IPrefabFactory _prefabFactory;

        public int numOfUnitDrones;
        public int numOfBasicDefenceBuildings;
        public int numOfAdvancedDefenceBuildings;

        protected const int DEFENCE_BUILDINGS_OFFSET_IN_M = 15;
        private const int DEFENCE_BUILDINGS_GROUP_GAP_IN_M = 1;
        private const float DEFENCE_BUILDING_SPACING_MULTIPLIER = 4;

        private const string DEFENCE_BUILDINGS_COST_PREFIX = "Building cost (drone seconds): ";
		
        public Camera Camera { get; private set; }

        public void Initialise(IPrefabFactory prefabFactory, IPrefabKey unitKey, IPrefabKey basicDefenceBuildingKey, IPrefabKey advancedDefenceBuildingKey)
        {
            Helper.AssertIsNotNull(prefabFactory, unitKey, basicDefenceBuildingKey, advancedDefenceBuildingKey);
            Assert.IsTrue(numOfUnitDrones > 0);
            Assert.IsTrue(numOfBasicDefenceBuildings >= 0);
            Assert.IsTrue(numOfAdvancedDefenceBuildings >= 0);


            _offensiveUnitKey = unitKey;
            _numOfDefenceBuildings = numOfBasicDefenceBuildings + numOfAdvancedDefenceBuildings;
            _helper = new TestUtils.Helper(numOfUnitDrones, buildSpeedMultiplier: BuildSpeedMultipliers.DEFAULT);
            _prefabFactory = prefabFactory;
            _defenceBuildings = new List<ITarget>(_numOfDefenceBuildings);
            _completedOffensiveUnits = new List<ITarget>();
            _numOfDefenceBuildngsDestroyed = 0;


            KillCountController killCount = transform.FindNamedComponent<KillCountController>("UnitKillCount");
            killCount.Initialise((int)FindUnitCost(unitKey));
            _unitKillCount = killCount;


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;


			SetupTexts(basicDefenceBuildingKey, advancedDefenceBuildingKey);
            CreateDefenceBuildings(basicDefenceBuildingKey, advancedDefenceBuildingKey);
        }

        private void CreateDefenceBuildings(IPrefabKey basicDefenceBuildingKey, IPrefabKey advancedDefenceBuildingKey)
        {
            IBuildableSpawner buildingSpawner = new BuildingSpawner(_prefabFactory, _helper);

            int basicBuildingsXPos = (int)transform.position.x + DEFENCE_BUILDINGS_OFFSET_IN_M;
            Vector2 basicBuildingsSpawnPos = new Vector2(basicBuildingsXPos, 0);
            TestUtils.BuildableInitialisationArgs basicBuildingsArgs = new TestUtils.BuildableInitialisationArgs(_helper, Faction.Reds, parentCruiserDirection: Direction.Left);

            IList<IBuildable> basicDefenceBuildings
                = buildingSpawner.SpawnBuildables(
                    basicDefenceBuildingKey,
                    numOfBasicDefenceBuildings,
				    basicBuildingsArgs,
                    basicBuildingsSpawnPos,
                    DEFENCE_BUILDING_SPACING_MULTIPLIER);

            float advancedBuildingsXPos = basicDefenceBuildings.Count != 0 ? basicDefenceBuildings.Last().Position.x : basicBuildingsXPos;
            Vector2 advancedBuildingsSpawnPosition = new Vector2(advancedBuildingsXPos + DEFENCE_BUILDINGS_GROUP_GAP_IN_M, 0);
            TestUtils.BuildableInitialisationArgs advancedBuildingsArgs = new TestUtils.BuildableInitialisationArgs(_helper, Faction.Reds, parentCruiserDirection: Direction.Left);

            IList<IBuildable> advancedDefenceBuildings =
                buildingSpawner.SpawnBuildables(
                    advancedDefenceBuildingKey,
                    numOfAdvancedDefenceBuildings,
					advancedBuildingsArgs,
                    advancedBuildingsSpawnPosition,
                    DEFENCE_BUILDING_SPACING_MULTIPLIER);

            IEnumerable<IBuildable> allDefenceBuildings = basicDefenceBuildings.Concat(advancedDefenceBuildings);
            foreach (IBuildable defenceBuilding in allDefenceBuildings)
            {
                defenceBuilding.CompletedBuildable += Building_CompletedBuildable;
                defenceBuilding.Destroyed += Building_Destroyed;
            }            
        }

        private void SetupTexts(IPrefabKey basicDefenceKey, IPrefabKey advancedDefenceKey)
        {
            // Show test case details
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = "Drones: " + numOfUnitDrones + "  AA: " + numOfBasicDefenceBuildings + "  SS: " + numOfAdvancedDefenceBuildings;

            // Defence building cost
            TextMesh buildingCostText = transform.FindNamedComponent<TextMesh>("BuildingCostText");
            int buildingCost
                = FindBuildingCost(basicDefenceKey, numOfBasicDefenceBuildings)
                + FindBuildingCost(advancedDefenceKey, numOfAdvancedDefenceBuildings);
            buildingCostText.text = DEFENCE_BUILDINGS_COST_PREFIX + buildingCost;
        }

        private int FindBuildingCost(IPrefabKey buildingKey, int numOfBuildings)
        {
            IBuildableWrapper<IBuilding> building = _prefabFactory.GetBuildingWrapperPrefab(buildingKey);
            return (int)building.Buildable.CostInDroneS * numOfBuildings;
        }

        private float FindUnitCost(IPrefabKey unitKey)
        {
            IBuildableWrapper<IUnit> unit = _prefabFactory.GetUnitWrapperPrefab(unitKey);
            return unit.Buildable.CostInDroneS;
        }

        private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            ITarget completedBuilding = sender.Parse<ITarget>();
            _defenceBuildings.Add(completedBuilding);

            if (_defenceBuildings.Count == _numOfDefenceBuildings)
            {
                _offensiveFactory = CreateFactory(_defenceBuildings);

                _offensiveFactory.CompletedBuildable += (factory, eventArgs) =>
                {
                    _offensiveFactory.StartBuildingUnit(_prefabFactory.GetUnitWrapperPrefab(_offensiveUnitKey));
                    _offensiveFactory.CompletedBuildingUnit += Factory_CompletedUnit;
                };

                _offensiveFactory.StartConstruction();
            }
        }

        protected abstract IFactory CreateFactory(IList<ITarget> defenceBuildings);

        private void Factory_CompletedUnit(object sender, UnitCompletedEventArgs e)
        {
            _completedOffensiveUnits.Add(e.CompletedUnit);
            e.CompletedUnit.Destroyed += Unit_Destroyed;
        }

        private void Unit_Destroyed(object sender, DestroyedEventArgs e)
        {
            _unitKillCount.KillCount++;
        }

        private void Building_Destroyed(object sender, DestroyedEventArgs e)
        {
            _numOfDefenceBuildngsDestroyed++;

            if (_numOfDefenceBuildngsDestroyed == _numOfDefenceBuildings)
            {
                OnAllDefenceBuildingsDestroyed();
            }
        }

        protected void OnAllDefenceBuildingsDestroyed()
        {
            // Stop producing units
            _offensiveFactory.StopBuildingUnit();

            int currentUnitKillCount = _unitKillCount.KillCount;

            // Destroy all units (because behaviour is undefined they have no more
            // targets, means the game is won).
            foreach (ITarget target in _completedOffensiveUnits)
            {
                if (!target.IsDestroyed)
                {
                    target.Destroy();
                }
            }

            // Do NOT count units destroyed programmatically at scenario end
            // towards the kill count.
            _unitKillCount.KillCount = currentUnitKillCount;
        }
    }
}
