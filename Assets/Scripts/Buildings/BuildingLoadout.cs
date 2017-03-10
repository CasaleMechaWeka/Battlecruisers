using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingKey
{
	public BuildingCategory Category { get; private set; }
	public string PrefabFileName { get; private set; }

	public BuildingKey(BuildingCategory category, string prefabFileName)
	{
		Category = category;
		PrefabFileName = prefabFileName;
	}
}

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

	private const string PREFABS_BASE_PATH = "Prefabs/Buildings/";

	private static class PrefabFolderNames
	{
		public const string FACTORIES = "Factories";
		public const string TACTICAL = "Tactical";
		public const string TURRETS  = "Turrets";
	}

	public BuildingLoadout(IList<BuildingKey> buildingKeys)
	{
		// FELIX TEMP
		GameObject o = (GameObject) Resources.Load("Prefabs/Buildings/Factories/AirFactory");
		Debug.Log("========= o: " + o);


		// Get Building prefabs for all building keys
		IDictionary<BuildingCategory, IList<Building>> buildingCategoryToGroups 
			= new Dictionary<BuildingCategory, IList<Building>>();

		foreach (BuildingKey buildingKey in buildingKeys)
		{
			string buildingPrefabPath = GetPrefabPath(buildingKey);
			Debug.Log($"buildingPrefabPath: {buildingPrefabPath}");

			GameObject prefabObject = Resources.Load(buildingPrefabPath) as GameObject;
			if (prefabObject == null)
			{
				throw new ArgumentException($"Invalid prefab path: {buildingPrefabPath}");
			}

			Building building = prefabObject.GetComponent<Building>();
			if (building == null)
			{
				throw new ArgumentException($"Prefab does not contain Building script.  Prefab path: {buildingPrefabPath}");
			}

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

	private string GetPrefabPath(BuildingKey buildingKey)
	{
		return PREFABS_BASE_PATH + GetBuildingFolderName(buildingKey.Category) + "/" + buildingKey.PrefabFileName;
	}

	private string GetBuildingFolderName(BuildingCategory buildingCategory)
	{
		switch (buildingCategory)
		{
			case BuildingCategory.Factory:
				return PrefabFolderNames.FACTORIES;
			case BuildingCategory.Tactical:
				return PrefabFolderNames.TACTICAL;
			case BuildingCategory.Turret:
				return PrefabFolderNames.TURRETS;
			default:
				throw new ArgumentException();
		}
	}
}
