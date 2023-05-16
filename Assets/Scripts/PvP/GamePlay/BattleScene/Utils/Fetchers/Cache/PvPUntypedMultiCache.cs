using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public class PvPUntypedMultiCache<TBase> : IPvPUntypedMultiCache<TBase> where TBase : class
    {
        private IDictionary<IPvPPrefabKey, TBase> _prefabs;

        public PvPUntypedMultiCache(IDictionary<IPvPPrefabKey, TBase> prefabs)
        {
            Assert.IsNotNull(prefabs);
            _prefabs = prefabs;
        }

        public TChild GetPrefab<TChild>(IPvPPrefabKey prefabKey) where TChild : class, TBase
        {
            Assert.IsNotNull(prefabKey);
            Assert.IsTrue(_prefabs.ContainsKey(prefabKey));

            TChild prefab = _prefabs[prefabKey] as TChild;
            Assert.IsNotNull(prefab);

            return prefab;
        }
    }
}