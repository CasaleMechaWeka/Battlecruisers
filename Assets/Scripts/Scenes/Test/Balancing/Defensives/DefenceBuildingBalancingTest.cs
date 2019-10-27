using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Balancing.Spawners;
using BattleCruisers.Scenes.Test.Balancing.Units;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
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
		private IList<IBuilding> _defenceBuildings;
        private int _numOfDefenceBuildngsDestroyed;
        private IDeferrer _deferrer;

        protected TestUtils.Helper _helper;

        public int numOfUnitDrones;
        public int numOfBasicDefenceBuildings;
        public int numOfAdvancedDefenceBuildings;

        protected const int DEFENCE_BUILDINGS_OFFSET_IN_M = 15;
        private const int DEFENCE_BUILDINGS_GROUP_GAP_IN_M = 1;
        private const float DEFENCE_BUILDING_SPACING_MULTIPLIER = 4;

        private const string DEFENCE_BUILDINGS_COST_PREFIX = "Building cost (drone seconds): ";
		
        public Camera Camera { get; private set; }

        public void Initialise(
            TestUtils.Helper baseHelper,
            IPrefabKey unitKey, 
            IPrefabKey basicDefenceBuildingKey, 
            IPrefabKey advancedDefenceBuildingKey)
        {
            Helper.AssertIsNotNull(baseHelper, unitKey, basicDefenceBuildingKey, advancedDefenceBuildingKey);
            Assert.IsTrue(numOfUnitDrones > 0);
            Assert.IsTrue(numOfBasicDefenceBuildings >= 0);
            Assert.IsTrue(numOfAdvancedDefenceBuildings >= 0);

            _offensiveUnitKey = unitKey;
            _numOfDefenceBuildings = numOfBasicDefenceBuildings + numOfAdvancedDefenceBuildings;
            _helper = new TestUtils.Helper(baseHelper, numOfUnitDrones, buildSpeedMultiplier: BuildSpeedMultipliers.DEFAULT);
            _defenceBuildings = new List<IBuilding>(_numOfDefenceBuildings);
            _completedOffensiveUnits = new List<ITarget>();
            _numOfDefenceBuildngsDestroyed = 0;

            _deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(_deferrer);

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
            ICruiser blueCruiser = _helper.CreateCruiser(Direction.Right, Faction.Blues);
            ICruiser redCruiser = _helper.CreateCruiser(Direction.Left , Faction.Reds);
            
            // Need to creat factory BEFORE defence buildings, so the defence buildings are captured
            // by the factory's GlobalTargetFinder
            _offensiveFactory = CreateFactory(blueCruiser, redCruiser);

            IBuildableSpawner buildingSpawner = new BuildingSpawner(_helper);

            int basicBuildingsXPos = (int)transform.position.x + DEFENCE_BUILDINGS_OFFSET_IN_M;
            Vector2 basicBuildingsSpawnPos = new Vector2(basicBuildingsXPos, 0);
            TestUtils.BuildableInitialisationArgs defenceBuildingArgs 
                = new TestUtils.BuildableInitialisationArgs(
                    _helper, 
                    Faction.Reds, 
                    parentCruiserDirection: Direction.Left,
                    updaterProvider: _helper.UpdaterProvider,
                    parentCruiser: redCruiser,
                    enemyCruiser: blueCruiser,
                    deferrer: _deferrer);

            IList<IBuildable> basicDefenceBuildings
                = buildingSpawner.SpawnBuildables(
                    basicDefenceBuildingKey,
                    numOfBasicDefenceBuildings,
				    defenceBuildingArgs,
                    basicBuildingsSpawnPos,
                    DEFENCE_BUILDING_SPACING_MULTIPLIER);

            float advancedBuildingsXPos = basicDefenceBuildings.Count != 0 ? basicDefenceBuildings.Last().Position.x : basicBuildingsXPos;
            Vector2 advancedBuildingsSpawnPosition = new Vector2(advancedBuildingsXPos + DEFENCE_BUILDINGS_GROUP_GAP_IN_M, 0);

            IList<IBuildable> advancedDefenceBuildings =
                buildingSpawner.SpawnBuildables(
                    advancedDefenceBuildingKey,
                    numOfAdvancedDefenceBuildings,
					defenceBuildingArgs,
                    advancedBuildingsSpawnPosition,
                    DEFENCE_BUILDING_SPACING_MULTIPLIER);

            IEnumerable<IBuildable> allDefenceBuildings = basicDefenceBuildings.Concat(advancedDefenceBuildings);
            foreach (IBuildable defenceBuilding in allDefenceBuildings)
            {
                defenceBuilding.CompletedBuildable += Building_CompletedBuildable;
                defenceBuilding.Destroyed += Building_Destroyed;

                // For the GlobalTargetFinder
                redCruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs((IBuilding)defenceBuilding));
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
            IBuildableWrapper<IBuilding> building = _helper.PrefabFactory.GetBuildingWrapperPrefab(buildingKey);
            return (int)building.Buildable.CostInDroneS * numOfBuildings;
        }

        private float FindUnitCost(IPrefabKey unitKey)
        {
            IBuildableWrapper<IUnit> unit = _helper.PrefabFactory.GetUnitWrapperPrefab(unitKey);
            return unit.Buildable.CostInDroneS;
        }

        private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            IBuilding completedBuilding = sender.Parse<IBuilding>();
            _defenceBuildings.Add(completedBuilding);

            if (_defenceBuildings.Count == _numOfDefenceBuildings)
            {
                _offensiveFactory.CompletedBuildable += (factory, eventArgs) =>
                {
                    _offensiveFactory.StartBuildingUnit(_helper.PrefabFactory.GetUnitWrapperPrefab(_offensiveUnitKey));
                    _offensiveFactory.UnitCompleted += Factory_CompletedUnit;
                };

                _offensiveFactory.StartConstruction();
            }
        }

        protected abstract IFactory CreateFactory(ICruiser parentCruiser, ICruiser enemyCruiser);

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
