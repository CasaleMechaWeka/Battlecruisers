using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public class PvPUntypedMultiCache<TBase> : IPvPUntypedMultiCache<TBase> where TBase : class
    {
        private IDictionary<IPrefabKey, TBase> _prefabs;

        public PvPUntypedMultiCache(IDictionary<IPrefabKey, TBase> prefabs)
        {
            Assert.IsNotNull(prefabs);
            _prefabs = prefabs;
        }

        public TChild GetPrefab<TChild>(IPrefabKey prefabKey) where TChild : class, TBase
        {
            Assert.IsNotNull(prefabKey);
            Assert.IsTrue(_prefabs.ContainsKey(prefabKey), prefabKey + " cannot be found");

            TChild prefab = _prefabs[prefabKey] as TChild;
            Assert.IsNotNull(prefab, prefabKey + " could not be retrieved as type " + typeof(TChild));

            return prefab;
        }

        public ICollection<IPrefabKey> GetKeys()
        {
            return _prefabs.Keys;
        }

        public ICollection<TBase> GetValues()
        {
            return _prefabs.Values;
        }
    }
}