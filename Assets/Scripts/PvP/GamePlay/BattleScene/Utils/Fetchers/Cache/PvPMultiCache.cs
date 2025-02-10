using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public class PvPMultiCache<TPrefab> : IPvPMultiCache<TPrefab> where TPrefab : class
    {
        private readonly IDictionary<IPrefabKey, TPrefab> _prefabs;

        public PvPMultiCache(IDictionary<IPrefabKey, TPrefab> prefabs)
        {
            Assert.IsNotNull(prefabs);
            _prefabs = prefabs;
        }

        public TPrefab GetPrefab(IPrefabKey prefabKey)
        {

            Assert.IsNotNull(prefabKey);
            if (!_prefabs.ContainsKey(prefabKey))
                Debug.LogWarning("PrefabKey ----------------> " + prefabKey.PrefabName + " is missing!");
            Assert.IsTrue(_prefabs.ContainsKey(prefabKey));
            return _prefabs[prefabKey];
        }

        public ICollection<IPrefabKey> GetKeys()
        {
            return _prefabs.Keys;
        }

        public ICollection<TPrefab> GetValues()
        {
            return _prefabs.Values;
        }

    }
}