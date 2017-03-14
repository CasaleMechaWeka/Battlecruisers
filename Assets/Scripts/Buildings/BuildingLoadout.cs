using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildings
{
	// FELIX Create interface
	// FELIX Create PrefabFetcher class?
	public class BuildingLoadout 
	{
		private readonly BuildingGroupFactory _buildingGroupFactory;

		public IList<BuildingGroup> BuildingGroups { get; private set; }

		// User needs to be able to build at least one building
		private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
		// Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
		private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

		public BuildingLoadout(PrefabFetcher prefabFetcher, IList<BuildingKey> buildingKeys)
		{
			// Get Building prefabs for all building keys
			IDictionary<BuildingCategory, IList<Building>> buildingCategoryToGroups 
				= new Dictionary<BuildingCategory, IList<Building>>();

			foreach (BuildingKey buildingKey in buildingKeys)
			{
				Building building = prefabFetcher.GetBuildingPrefab(buildingKey);

				if (!buildingCategoryToGroups.ContainsKey(buildingKey.Category))
				{
					buildingCategoryToGroups[buildingKey.Category] = new List<Building>();
				}

				buildingCategoryToGroups[buildingKey.Category].Add(building);
			}

			// Create BuildingGroups
			_buildingGroupFactory = new BuildingGroupFactory();
			BuildingGroups = new List<BuildingGroup>(buildingCategoryToGroups.Count);

			foreach (KeyValuePair<BuildingCategory, IList<Building>> categoryToBuildings in buildingCategoryToGroups)
			{
				BuildingGroup group = _buildingGroupFactory.CreateBuildingGroup(categoryToBuildings.Key, categoryToBuildings.Value);
				BuildingGroups.Add(group);
			}
		}
	}
}
