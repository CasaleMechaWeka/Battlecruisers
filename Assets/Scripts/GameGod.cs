using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Buildables.Units;
using BattleCruisers.TargetFinders;
using BattleCruisers.Utils;
using BattleCruisers.UI;
using BattleCruisers.UI.BuildMenus;
using BattleCruisers.UI.BuildingDetails;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers
{
	/// <summary>
	/// Initialises everything :D
	/// </summary>
	public class GameGod : MonoBehaviour 
	{
		private BuildingGroupFactory _buildingGroupFactory;
		private Bot _bot;

		public BuildableFactory buildableFactory;
		public UIManager uiManager;
		public UIFactory uiFactory;
		public BuildMenuController buildMenuController;
		public BuildableDetailsController buildableDetailsController;
		public Cruiser friendlyCruiser;
		public Cruiser enemyCruiser;
		
		// User needs to be able to build at least one building
		private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
		// Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
		private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

		void Awake()
		{
			Assert.raiseExceptions = true;


			// Common setup
			_buildingGroupFactory = new BuildingGroupFactory();
			
			PrefabFetcher prefabFetcher = new PrefabFetcher();
			buildableFactory.Initialise(prefabFetcher);


			// Player cruiser
			TargetFinderFactory playerCruiserTargetFinderFactory = new TargetFinderFactory(enemyCruiser);
			IDroneManager droneManager = new DroneManager();
			IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
			friendlyCruiser.Initialise(droneManager, droneConsumerProvider, playerCruiserTargetFinderFactory);
			friendlyCruiser.direction = Direction.Right;


			// UI
			ISpriteFetcher spriteFetcher = new SpriteFetcher();
			buildableDetailsController.Initialise(droneManager, spriteFetcher);
			uiFactory.Initialise(spriteFetcher, droneManager);

			Loadout loadout = CreateLoadout();

			IDictionary<BuildingCategory, IList<Building>> buildings = GetBuildingsFromKeys(loadout, friendlyCruiser, enemyCruiser);
			IList<BuildingGroup> buildingGroups = CreateBuildingGroups(buildings);
			IDictionary<UnitCategory, IList<Unit>> units = GetUnitsFromKeys(loadout, friendlyCruiser, enemyCruiser);
			buildMenuController.Initialise(buildingGroups, units);


			// AI cruiser
			TargetFinderFactory aiCruiserTargetFinderFactory = new TargetFinderFactory(friendlyCruiser);
			IDroneManager aiDroneManager = new DroneManager();
			IDroneConsumerProvider aiDroneConsumerProvider = new DroneConsumerProvider(aiDroneManager);
			enemyCruiser.direction = Direction.Left;
			enemyCruiser.Initialise(aiDroneManager, aiDroneConsumerProvider, aiCruiserTargetFinderFactory);


			// AI
			IList<BuildingKey> buildOrder = GetBuildOrder();
			_bot = new Bot(enemyCruiser, friendlyCruiser, buildOrder, buildableFactory);
//			Invoke("StartBot", 10);
			Invoke("StartBot", 2);
		}

		private void StartBot()
		{
			_bot.Start();
		}

		// FELIX  Should not be hardcoded.  User loadouts should be in db?
		private Loadout CreateLoadout()
		{
			// Factories
			IList<BuildingKey> factories = new List<BuildingKey>();
			factories.Add(new BuildingKey(BuildingCategory.Factory, "AirFactory"));
			factories.Add(new BuildingKey(BuildingCategory.Factory, "NavalFactory"));
			factories.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));
			
			// Tactical
			IList<BuildingKey> tactical = new List<BuildingKey>();
			tactical.Add(new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"));

			// Defence
			IList<BuildingKey> defence = new List<BuildingKey>();
			defence.Add(new BuildingKey(BuildingCategory.Defence, "ShootyTurret"));

			// Offence
			IList<BuildingKey> offence = new List<BuildingKey>();
			offence.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));

			// Support
			IList<BuildingKey> support = new List<BuildingKey>();

			// Ultra buildings
			IList<BuildingKey> ultraBuildings = new List<BuildingKey>();

			// Aircraft
			IList<UnitKey> aircraft = new List<UnitKey>();
			aircraft.Add(new UnitKey(UnitCategory.Aircraft, "Bomber"));

			// Ships
			IList<UnitKey> ships = new List<UnitKey>();
			ships.Add(new UnitKey(UnitCategory.Naval, "AttackBoat"));
			ships.Add(new UnitKey(UnitCategory.Naval, "AttackBoat2"));

			// Ultra units
			IList<UnitKey> ultraUnits = new List<UnitKey>();

			return new Loadout(
				factories,
				tactical,
				defence,
				offence,
				support,
				ultraBuildings,
				aircraft,
				ships,
				ultraUnits);
		}

		private IDictionary<BuildingCategory, IList<Building>> GetBuildingsFromKeys(Loadout loadout, Cruiser parentCruiser, Cruiser hostileCruiser)
		{
			IDictionary<BuildingCategory, IList<Building>> categoryToBuildings = new Dictionary<BuildingCategory, IList<Building>>();
			
			foreach (BuildingCategory category in Enum.GetValues(typeof(BuildingCategory)))
			{
				IList<BuildingKey> buildingKeys = loadout.GetBuildings(category);
				
				if (buildingKeys.Count != 0)
				{
					IList<Building> buildings = new List<Building>();
					categoryToBuildings[category] = buildings;
					
					foreach (BuildingKey buildingKey in buildingKeys)
					{
						Building building = buildableFactory.GetBuildingPrefab(buildingKey);
						categoryToBuildings[buildingKey.Category].Add(building);
					}
				}
			}

			return categoryToBuildings;
		}

		private IList<BuildingGroup> CreateBuildingGroups(IDictionary<BuildingCategory, IList<Building>> buildingCategoryToGroups)
		{
			IList<BuildingGroup> buildingGroups = new List<BuildingGroup>();

			foreach (KeyValuePair<BuildingCategory, IList<Building>> categoryToBuildings in buildingCategoryToGroups)
			{
				BuildingGroup group = _buildingGroupFactory.CreateBuildingGroup(categoryToBuildings.Key, categoryToBuildings.Value);
				buildingGroups.Add(group);
			}
			
			if (buildingGroups.Count < MIN_NUM_OF_BUILDING_GROUPS
				|| buildingGroups.Count > MAX_NUM_OF_BUILDING_GROUPS)
			{
				throw new InvalidProgramException();
			}
			
			return buildingGroups;
		}
	
		private IDictionary<UnitCategory, IList<Unit>> GetUnitsFromKeys(Loadout loadout, Cruiser parentCruiser, Cruiser hostileCruiser)
		{
			IDictionary<UnitCategory, IList<Unit>> categoryToUnits = new Dictionary<UnitCategory, IList<Unit>>();

			foreach (UnitCategory unitCategory in Enum.GetValues(typeof(UnitCategory)))
			{
				IList<UnitKey> unitKeys = loadout.GetUnits(unitCategory);

				if (unitKeys.Count != 0)
				{
					categoryToUnits[unitCategory] = GetUnits(unitKeys, parentCruiser, hostileCruiser);
				}
			}

			return categoryToUnits;
		}

		private IList<Unit> GetUnits(IList<UnitKey> unitKeys, Cruiser parentCruiser, Cruiser hostileCruiser)
		{
			IList<Unit> units = new List<Unit>(unitKeys.Count);

			foreach (UnitKey unitKey in unitKeys)
			{
				Unit unit = buildableFactory.GetUnitPrefab(unitKey);
				units.Add(unit);
			}

			return units;
		}

		// FELIX  Don't hardcode.  Database?
		private IList<BuildingKey> GetBuildOrder()
		{
			IList<BuildingKey> buildOrder = new List<BuildingKey>();

			buildOrder.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "ShootyTurret"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Factory, "NavalFactory"));
//			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "ShootyTurret"));
//			buildOrder.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"));

			return buildOrder;
		}
	}
}
