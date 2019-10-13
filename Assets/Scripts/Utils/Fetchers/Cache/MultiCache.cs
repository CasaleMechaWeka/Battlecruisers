using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    public class MultiCache<TPrefab> : IMultiCache<TPrefab> where TPrefab : class
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
            Assert.IsTrue(_prefabs.ContainsKey(prefabKey));
            return _prefabs[prefabKey];
        }
    }
}