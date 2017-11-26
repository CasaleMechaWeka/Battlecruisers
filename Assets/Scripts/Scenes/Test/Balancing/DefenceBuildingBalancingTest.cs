using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;
using UnityEngine.Assertions;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public abstract class DefenceBuildingBalancingTest : MonoBehaviour
    {
        private int _numOfDefenceBuildings;
        private IPrefabKey _offensiveUnitKey;
        private IFactory _offensiveFactory;
        private IList<ITarget> _completedOffensiveUnits;
        private ITextMesh _unitKillCountText, _totalUnitsCostText;
        private float _unitCostInDroneS;
		private IList<ITarget> _defenceBuildings;
        private int _numOfDefenceBuildngsDestroyed;

        protected TestUtils.Helper _helper;
        protected IPrefabFactory _prefabFactory;

        public int numOfUnitDrones;
        public int numOfBasicDefenceBuildings;
        public int numOfAdvancedDefenceBuildings;

        protected const int DEFENCE_BUILDINGS_OFFSET_IN_M = 15;
        private const int DEFENCE_BUILDINGS_GAP_IN_M = 2;

        private const string UNIT_KILL_COUNT_PREFIX = "Dead units: ";
        private const string UNIT_COST_PREFIX = "Unit cost (drone seconds): ";
        private const string DEFENCE_BUILDINGS_COST_PREFIX = "Building cost (drone secodns): ";
		
        public Camera Camera { get; private set; }

        private int _unitKillCount;
        private int UnitKillCount
        {
            get { return _unitKillCount; }
            set
            {
                _unitKillCount = value;
                _unitKillCountText.Text = UNIT_KILL_COUNT_PREFIX + _unitKillCount;
                _totalUnitsCostText.Text = UNIT_COST_PREFIX + (_unitKillCount * _unitCostInDroneS);
            }
        }

        public void Initialise(IPrefabFactory prefabFactory, IPrefabKey unitKey, IPrefabKey basicDefenceBuildingKey, IPrefabKey advancedDefenceBuildingKey)
        {
            Helper.AssertIsNotNull(prefabFactory, unitKey, basicDefenceBuildingKey, advancedDefenceBuildingKey);
            Assert.IsTrue(numOfUnitDrones > 0);
            Assert.IsTrue(numOfBasicDefenceBuildings >= 0);
            Assert.IsTrue(numOfAdvancedDefenceBuildings >= 0);


            _offensiveUnitKey = unitKey;
            _numOfDefenceBuildings = numOfBasicDefenceBuildings + numOfAdvancedDefenceBuildings;
            _helper = new TestUtils.Helper(numOfDrones: numOfUnitDrones);
            _prefabFactory = prefabFactory;
            _defenceBuildings = new List<ITarget>(_numOfDefenceBuildings);
            _completedOffensiveUnits = new List<ITarget>();
            _unitCostInDroneS = FindUnitCost(_offensiveUnitKey);
            _numOfDefenceBuildngsDestroyed = 0;


            SetupTexts(basicDefenceBuildingKey, advancedDefenceBuildingKey);


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;


            // Create defence buildings
            int originalOffsetInM = (int)transform.position.x + DEFENCE_BUILDINGS_OFFSET_IN_M;
            int currentOffsetInM = CreateBuildings(basicDefenceBuildingKey, numOfBasicDefenceBuildings, originalOffsetInM);
            CreateBuildings(advancedDefenceBuildingKey, numOfAdvancedDefenceBuildings, currentOffsetInM);
        }

        private void SetupTexts(IPrefabKey antiAirKey, IPrefabKey samSiteKey)
        {
            // Show test case details
            TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            detailsText.text = "Drones: " + numOfUnitDrones + "  AA: " + numOfBasicDefenceBuildings + "  SS: " + numOfAdvancedDefenceBuildings;

            // Anti air building cost
            TextMesh buildingCostText = transform.FindNamedComponent<TextMesh>("BuildingCostText");
            int buildingCost
                = FindBuildingCost(antiAirKey, numOfBasicDefenceBuildings)
                + FindBuildingCost(samSiteKey, numOfAdvancedDefenceBuildings);
            buildingCostText.text = DEFENCE_BUILDINGS_COST_PREFIX + buildingCost;

            // Unit cost
            TextMesh unitCostText = transform.FindNamedComponent<TextMesh>("UnitCostText");
            _totalUnitsCostText = new TextMeshWrapper(unitCostText);

			// Unit kill count
            TextMesh uniKillCountText = transform.FindNamedComponent<TextMesh>("UnitKillCountText");
            _unitKillCountText = new TextMeshWrapper(uniKillCountText);
			UnitKillCount = 0;
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

                currentOffsetInM += DEFENCE_BUILDINGS_GAP_IN_M;
                building.CompletedBuildable += Building_CompletedBuildable;
				building.Destroyed += Building_Destroyed;
                building.StartConstruction();
            }

            return currentOffsetInM;
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
                    _offensiveFactory.UnitWrapper = _prefabFactory.GetUnitWrapperPrefab(_offensiveUnitKey);
                    _offensiveFactory.CompletedBuildingUnit += Factory_CompletedUnit;
                };
                _offensiveFactory.StartConstruction();
            }
        }

        protected abstract IFactory CreateFactory(IList<ITarget> defenceBuildings);

        private void Factory_CompletedUnit(object sender, CompletedConstructionEventArgs e)
        {
            _completedOffensiveUnits.Add(e.Buildable);
            e.Buildable.Destroyed += Unit_Destroyed;
        }

        private void Unit_Destroyed(object sender, DestroyedEventArgs e)
        {
            UnitKillCount++;
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
            _offensiveFactory.UnitWrapper = null;

            int currentUnitKillCount = UnitKillCount;

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
            UnitKillCount = currentUnitKillCount;
        }
    }
}
