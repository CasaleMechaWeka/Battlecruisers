using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    // FELIX  Avoid duplicate code with:  FactoryTestGod & DefenceBuildingBalancingTest
    public class ShipVsShipBalancingTest : MonoBehaviour, ITestScenario
    {
        private IPrefabKey _leftShipKey, _rightShipKey;
        private IFactory _leftFactory, _rightFactory;
        private ITextMesh _leftShipKillCountText, _totalLeftShipCostText, _rightShipKillCountText, _totalRightShipCostText;
		private IList<ITarget> _completedShips;

        protected TestUtils.Helper _helper;
        protected IPrefabFactory _prefabFactory;

        public int numOfDrones;

        // FELIX  Update
        private const string UNIT_KILL_COUNT_PREFIX = "Dead units: ";
        private const string UNIT_COST_PREFIX = "Unit cost (drone seconds): ";
        private const string DEFENCE_BUILDINGS_COST_PREFIX = "Building cost (drone secodns): ";

        public Camera Camera { get; private set; }

        //private int _unitKillCount;
        //private int UnitKillCount
        //{
        //    get { return _unitKillCount; }
        //    set
        //    {
        //        _unitKillCount = value;
        //        _unitKillCountText.Text = UNIT_KILL_COUNT_PREFIX + _unitKillCount;
        //        _totalUnitsCostText.Text = UNIT_COST_PREFIX + (_unitKillCount * _unitCostInDroneS);
        //    }
        //}

        public void Initialise(IPrefabFactory prefabFactory, IPrefabKey leftShipKey, IPrefabKey rightShipKey)
        {
            Helper.AssertIsNotNull(prefabFactory, leftShipKey, rightShipKey);
            Assert.IsTrue(numOfDrones > 0);


            _leftShipKey = leftShipKey;
            _rightShipKey = rightShipKey;

            _helper = new TestUtils.Helper(numOfDrones: numOfDrones);
            _prefabFactory = prefabFactory;
            _completedShips = new List<ITarget>();


            // FELIX
            //SetupTexts(rightShipKey, advancedDefenceBuildingKey);


            // FELIX
            IFactory[] factories = GetComponentsInChildren<IFactory>();
            Assert.IsTrue(factories.Length == 2);
            factories = factories.OrderBy(factory => factory.Position.x).ToArray();

            _leftFactory = factories[0];
            _rightFactory = factories[1];

            InitialiseFactory(_leftFactory, Faction.Reds, Direction.Right, _leftShipKey);
            InitialiseFactory(_rightFactory, Faction.Blues, Direction.Left, _rightShipKey);


            // Hide camera
            Camera = GetComponentInChildren<Camera>();
            Camera.enabled = false;
        }

        private void InitialiseFactory(IFactory factory, Faction faction, Direction facingDirection, IPrefabKey unitKey)
        {
            _helper
                .InitialiseBuilding(
                    factory,
                    faction,
                    parentCruiserDirection: facingDirection);

            factory.CompletedBuildable += (sender, e) =>
			{
                factory.UnitWrapper = _prefabFactory.GetUnitWrapperPrefab(unitKey);
                factory.CompletedBuildingUnit += Factory_CompletedUnit;
			};

            factory.StartConstruction();
        }

        private void SetupTexts(IPrefabKey antiAirKey, IPrefabKey samSiteKey)
        {
            //// Show test case details
            //TextMesh detailsText = transform.FindNamedComponent<TextMesh>("DetailsText");
            //detailsText.text = "Drones: " + numOfDrones + "  AA: " + numOfBasicDefenceBuildings + "  SS: " + numOfAdvancedDefenceBuildings;

            //// Anti air building cost
            //TextMesh buildingCostText = transform.FindNamedComponent<TextMesh>("BuildingCostText");
            //int buildingCost
            //    = FindBuildingCost(antiAirKey, numOfBasicDefenceBuildings)
            //    + FindBuildingCost(samSiteKey, numOfAdvancedDefenceBuildings);
            //buildingCostText.text = DEFENCE_BUILDINGS_COST_PREFIX + buildingCost;

            //// Unit cost
            //TextMesh unitCostText = transform.FindNamedComponent<TextMesh>("UnitCostText");
            //_totalUnitsCostText = new TextMeshWrapper(unitCostText);

            //// Unit kill count
            //TextMesh uniKillCountText = transform.FindNamedComponent<TextMesh>("UnitKillCountText");
            //_unitKillCountText = new TextMeshWrapper(uniKillCountText);
            //UnitKillCount = 0;
        }

        private float FindUnitCost(IPrefabKey unitKey)
        {
            IBuildableWrapper<IUnit> unit = _prefabFactory.GetUnitWrapperPrefab(unitKey);
            return unit.Buildable.NumOfDronesRequired * unit.Buildable.BuildTimeInS;
        }

        private void Factory_CompletedUnit(object sender, CompletedConstructionEventArgs e)
        {
            _completedShips.Add(e.Buildable);
            e.Buildable.Destroyed += Ship_Destroyed;
        }

        private void Ship_Destroyed(object sender, DestroyedEventArgs e)
        {
            // FELIX
            //UnitKillCount++;
        }

        protected void OnScenarioComplete()
        {
            // Stop producing units
            _leftFactory.UnitWrapper = null;
            _rightFactory.UnitWrapper = null;

            //int currentUnitKillCount = UnitKillCount;

            //// Destroy all units (because behaviour is undefined they have no more
            //// targets, means the game is won).
            //foreach (ITarget target in _completedShips)
            //{
            //    if (!target.IsDestroyed)
            //    {
            //        target.Destroy();
            //    }
            //}

            //// Do NOT count units destroyed programmatically at scenario end
            //// towards the kill count.
            //UnitKillCount = currentUnitKillCount;
        }
    }
}
