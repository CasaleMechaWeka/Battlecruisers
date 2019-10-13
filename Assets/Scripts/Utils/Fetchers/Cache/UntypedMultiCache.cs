using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers.Cache
{
    public class UntypedMultiCache<TBase> : IUntypedMultiCache<TBase> where TBase : class
    {
        private IDictionary<IPrefabKey, TBase> _prefabs;

        public UntypedMultiCache(IDictionary<IPrefabKey, TBase> prefabs)
        {
            Assert.IsNotNull(prefabs);
            _prefabs = prefabs;
        }

        public TChild GetPrefab<TChild>(IPrefabKey prefabKey) where TChild : class, TBase
        {
            Assert.IsNotNull(prefabKey);
            Assert.IsTrue(_prefabs.ContainsKey(prefabKey));

            TChild prefab = _prefabs[prefabKey] as TChild;
            Assert.IsNotNull(prefab);

            return prefab;
        }
    }
}