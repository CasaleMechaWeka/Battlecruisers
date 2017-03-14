using BattleCruisers.Buildings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils
{
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

	public class PrefabFetcher
	{
		private const string PREFABS_BASE_PATH = "Prefabs/Buildings/";

		private static class PrefabFolderNames
		{
			public const string FACTORIES = "Factories";
			public const string TACTICAL = "Tactical";
			public const string DEFENCE  = "Defence";
			public const string OFFENCE  = "Offence";
		}

		public Building GetBuildingPrefab(BuildingKey buildingKey)
		{
			string buildingPrefabPath = GetPrefabPath(buildingKey);

			// FELIX TEMP
	//		Debug.Log($"buildingPrefabPath: {buildingPrefabPath}");

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

			return building;
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
				case BuildingCategory.Defence:
					return PrefabFolderNames.DEFENCE;
				case BuildingCategory.Offence:
					return PrefabFolderNames.OFFENCE;
				default:
					throw new ArgumentException();
			}
		}
	}
}
