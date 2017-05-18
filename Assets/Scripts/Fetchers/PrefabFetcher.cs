using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers.PrefabKeys;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Fetchers
{
	public class PrefabFetcher
	{
		public BuildingWrapper GetBuildingPrefab(BuildingKey buildingKey)
		{
			string buildingPrefabPath = GetBuildingPath(buildingKey);
			return GetPrefab<BuildingWrapper>(buildingPrefabPath);
		}

		public UnitWrapper GetUnitPrefab(UnitKey unitKey)
		{
			string unitPrefabPath = GetUnitPath(unitKey);
			return GetPrefab<UnitWrapper>(unitPrefabPath);
		}

		private T GetPrefab<T>(string prefabPath)
		{
			GameObject gameObject = Resources.Load(prefabPath) as GameObject;
			if (gameObject == null)
			{
				throw new ArgumentException($"Invalid prefab path: {prefabPath}");
			}

			T prefabObject = gameObject.GetComponent<T>();
			if (prefabObject == null)
			{
				throw new ArgumentException($"Prefab does not contain a component of type: {typeof(T)}.  Prefab path: {prefabPath}");
			}
			return prefabObject;
		}

		// FELIX
		private string GetBuildingPath(BuildingKey buildingKey)
		{
			return "";
//			return BUILDINGS_BASE_PATH + GetBuildingFolderName(buildingKey.Category) + "/" + buildingKey.PrefabFileName;
		}

		// FELIX
		private string GetUnitPath(UnitKey unitKey)
		{
			return "";
//			return UNITS_BASE_PATH + GetUnitFolderName(unitKey.Category) + "/" + unitKey.PrefabFileName;
		}
	}
}
