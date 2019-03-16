using System;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public class PrefabFetcher
	{
		public T GetPrefab<T>(IPrefabKey prefabKey) where T : class
		{
			GameObject gameObject = Resources.Load(prefabKey.PrefabPath) as GameObject;
			if (gameObject == null)
			{
				throw new ArgumentException("Invalid prefab path: " + prefabKey.PrefabPath);
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
