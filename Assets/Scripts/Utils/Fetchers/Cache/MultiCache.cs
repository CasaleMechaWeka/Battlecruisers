using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    public class MultiCache<TPrefab> where TPrefab : class
    {
        private readonly IDictionary<IPrefabKey, TPrefab> _prefabs;

        public MultiCache(IDictionary<IPrefabKey, TPrefab> prefabs)
        {
            Assert.IsNotNull(prefabs);
            _prefabs = prefabs;
        }

        public TPrefab GetPrefab(IPrefabKey prefabKey)
        {
            Assert.IsNotNull(prefabKey);
            //Assert.IsTrue(_prefabs.ContainsKey(prefabKey));
            if (_prefabs.ContainsKey(prefabKey))
                return _prefabs[prefabKey];
            else
            {
                Debug.LogError($"PrefabKey '{prefabKey.PrefabName}' with path '{prefabKey.PrefabPath}' is missing from cache! This prefab may not be configured as an Addressable Asset.");
                throw new KeyNotFoundException($"Required prefab '{prefabKey.PrefabName}' not found in cache. Check Addressable Assets configuration.");
            }
        }
    }
}