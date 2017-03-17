using BattleCruisers.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Units;
using BattleCruisers.Utils;
using BattleCruisers.UI;
using BattleCruisers.UI.BuildMenus;
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
		private PrefabFetcher _prefabFetcher;
		private BuildingGroupFactory _buildingGroupFactory;

		public BuildingFactory buildingFactory;
		public UIManager uiManager;
		public BuildMenuController buildMenuController;
		public Cruiser friendlyCruiser;
		public Cruiser enemyCruiser;
		
		// User needs to be able to build at least one building
		private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
		// Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
		private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

		void Awake()
		{
			Assert.raiseExceptions = true;

			_prefabFetcher = new PrefabFetcher();
			_buildingGroupFactory = new BuildingGroupFactory();

			buildingFactory.Initialise(uiManager, _prefabFetcher);
			friendlyCruiser.direction = Direction.Right;
			enemyCruiser.direction = Direction.Left;

			Loadout loadout = CreateLoadout();

			IDictionary<BuildingCategory, IList<Building>> buildings = GetBuildingsFromKeys(loadout, friendlyCruiser);
			IList<BuildingGroup> buildingGroups = CreateBuildingGroups(buildings);
			buildMenuController.Initialise(buildingGroups);

			IDictionary<UnitCategory, IList<Unit>> units = GetUnitsFromKeys(loadout);
			buildingFactory.Units = units;
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
			tactical.Add(new BuildingKey(BuildingCategory.Tactical, "Shield"));

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

			// Ships
			IList<UnitKey> ships = new List<UnitKey>();
			ships.Add(new UnitKey(UnitCategory.Naval, "AttackBoat"));

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

		private IDictionary<BuildingCategory, IList<Building>> GetBuildingsFromKeys(Loadout loadout, Cruiser parentCruiser)
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
						Building building = buildingFactory.GetBuildingPrefab(buildingKey, parentCruiser, enemyCruiser);
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
	
		private IDictionary<UnitCategory, IList<Unit>> GetUnitsFromKeys(Loadout loadout)
		{
			IDictionary<UnitCategory, IList<Unit>> categoryToUnits = new Dictionary<UnitCategory, IList<Unit>>();

			foreach (UnitCategory unitCategory in Enum.GetValues(typeof(UnitCategory)))
			{
				IList<UnitKey> unitKeys = loadout.GetUnits(unitCategory);

				if (unitKeys.Count != 0)
				{
					categoryToUnits[unitCategory] = GetUnits(unitKeys);
				}
			}

			return categoryToUnits;
		}

		private IList<Unit> GetUnits(IList<UnitKey> unitKeys)
		{
			IList<Unit> units = new List<Unit>(unitKeys.Count);

			foreach (UnitKey unitKey in unitKeys)
			{
				// FELIX  Create unit factory?
				Unit unit = _prefabFetcher.GetUnitPrefab(unitKey);
				units.Add(unit);
			}

			return units;
		}
	}
}
