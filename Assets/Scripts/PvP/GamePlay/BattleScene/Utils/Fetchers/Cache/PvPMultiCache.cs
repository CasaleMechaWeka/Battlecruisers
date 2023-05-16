using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public class PvPMultiCache<TPrefab> : IPvPMultiCache<TPrefab> where TPrefab : class
    {
        private readonly IDictionary<IPvPPrefabKey, TPrefab> _prefabs;

        public PvPMultiCache(IDictionary<IPvPPrefabKey, TPrefab> prefabs)
        {
            Assert.IsNotNull(prefabs);
            _prefabs = prefabs;
        }

        public TPrefab GetPrefab(IPvPPrefabKey prefabKey)
        {
            Assert.IsNotNull(prefabKey);
            Assert.IsTrue(_prefabs.ContainsKey(prefabKey));
            return _prefabs[prefabKey];
        }
    }
}