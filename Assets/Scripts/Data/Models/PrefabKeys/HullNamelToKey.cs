using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class HullNamelToKey : IHullNameToKey
    {
        private readonly IDictionary<string, HullKey> _hullNameToKey;

        public HullNamelToKey(IList<HullKey> keys, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(keys, prefabFactory);

            _hullNameToKey = new Dictionary<string, HullKey>();

            foreach (HullKey key in keys)
            {
                ICruiser hullPrefab = prefabFactory.GetCruiserPrefab(key);
                _hullNameToKey.Add(hullPrefab.Name, key);
            }
        }

        public HullKey GetKey(string hullName)
        {
            Assert.IsTrue(_hullNameToKey.ContainsKey(hullName));
            return _hullNameToKey[hullName];
        }
    }
}