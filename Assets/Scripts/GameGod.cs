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

			InitializeBuildMenuController();
		}

		// FELIX  Don't hardcode
		private void InitializeBuildMenuController()
		{
			IList<BuildingKey> buildingKeys = new List<BuildingKey>();

			// Factories
			buildingKeys.Add(new BuildingKey(BuildingCategory.Factory, "AirFactory"));
			buildingKeys.Add(new BuildingKey(BuildingCategory.Factory, "NavalFactory"));
			buildingKeys.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));

			// Tactical
			buildingKeys.Add(new BuildingKey(BuildingCategory.Tactical, "Shield"));

			// Defence
			buildingKeys.Add(new BuildingKey(BuildingCategory.Defence, "ShootyTurret"));

			// Offence
			buildingKeys.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));

			IList<BuildingGroup> buildingGroups = CreateBuildingGroups(buildingKeys, friendlyCruiser, enemyCruiser);
			buildMenuController.Initialise(buildingGroups);
		}


		private IList<BuildingGroup> CreateBuildingGroups(IList<BuildingKey> buildingKeys, Cruiser parentCruiser, Cruiser enemyCruiser)
		{
			// Get Building prefabs for all building keys
			IDictionary<BuildingCategory, IList<Building>> buildingCategoryToGroups 
				= new Dictionary<BuildingCategory, IList<Building>>();

			foreach (BuildingKey buildingKey in buildingKeys)
			{
				Building building = buildingFactory.GetBuildingPrefab(buildingKey, parentCruiser, enemyCruiser);

				if (!buildingCategoryToGroups.ContainsKey(buildingKey.Category))
				{
					buildingCategoryToGroups[buildingKey.Category] = new List<Building>();
				}

				buildingCategoryToGroups[buildingKey.Category].Add(building);
			}

			// Create BuildingGroups
			_buildingGroupFactory = new BuildingGroupFactory();
			IList<BuildingGroup> buildingGroups = new List<BuildingGroup>(buildingCategoryToGroups.Count);

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
	}
}
