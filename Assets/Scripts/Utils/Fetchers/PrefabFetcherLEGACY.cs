using BattleCruisers.Data.Models.PrefabKeys;
using System;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public class PrefabFetcherLEGACY : IPrefabFetcherLEGACY
    {
		public TPrefab GetPrefab<TPrefab>(IPrefabKey prefabKey) where TPrefab : class
		{
			GameObject gameObject = Resources.Load(prefabKey.PrefabPath) as GameObject;
			if (gameObject == null)
			{
				throw new ArgumentException("Invalid prefab path: " + prefabKey.PrefabPath);
			}

			TPrefab prefabObject = gameObject.GetComponent<TPrefab>();
			if (prefabObject == null)
			{
				throw new ArgumentException($"Prefab does not contain a component of type: {typeof(TPrefab)}.  Prefab path: {prefabKey.PrefabPath}");
			}
			return prefabObject;
		}
	}
}
