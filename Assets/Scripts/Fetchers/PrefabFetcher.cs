using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
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

		public Cruiser GetCruiserPrefab(HullKey hullKey)
		{
			return GetPrefab<Cruiser>(hullKey);
		}

		private T GetPrefab<T>(PrefabKey prefabKey)
		{
			GameObject gameObject = Resources.Load(prefabKey.PrefabPath) as GameObject;
			if (gameObject == null)
			{
				throw new ArgumentException("Invalid prefab path: " + prefabKey.PrefabPath);
			}

			T prefabObject = gameObject.GetComponent<T>();
			if (prefabObject == null)
			{
				throw new ArgumentException(string.Format("Prefab does not contain a component of type: {0}.  Prefab path: {1}", typeof(T), prefabKey.PrefabPath));
			}
			return prefabObject;
		}
	}
}
