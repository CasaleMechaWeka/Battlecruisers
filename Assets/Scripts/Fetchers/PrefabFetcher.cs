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
			return GetPrefab<BuildingWrapper>(buildingKey);
		}

		public UnitWrapper GetUnitPrefab(UnitKey unitKey)
		{
			return GetPrefab<UnitWrapper>(unitKey);
		}

		private T GetPrefab<T>(PrefabKey prefabKey)
		{
			GameObject gameObject = Resources.Load(prefabKey.PrefabPath) as GameObject;
			if (gameObject == null)
			{
				throw new ArgumentException($"Invalid prefab path: {prefabKey.PrefabPath}");
			}

			T prefabObject = gameObject.GetComponent<T>();
			if (prefabObject == null)
			{
				throw new ArgumentException($"Prefab does not contain a component of type: {typeof(T)}.  Prefab path: {prefabKey.PrefabPath}");
			}
			return prefabObject;
		}
	}
}
