using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class HullNameToKey : IHullNameToKey
    {
        private readonly IDictionary<string, HullKey> _hullNameToKey;
        private readonly IDictionary<string, HullKey> _hullTypeToKey;

        public HullNameToKey(IList<HullKey> keys, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(keys, prefabFactory);

            _hullNameToKey = new Dictionary<string, HullKey>();
            _hullTypeToKey = new Dictionary<string, HullKey>();

            foreach (HullKey key in keys)
            {
                ICruiser hullPrefab = prefabFactory.GetCruiserPrefab(key);
                _hullNameToKey.Add(hullPrefab.Name, key);
                _hullTypeToKey.Add(key.PrefabName, key);
            }
        }

        public HullKey GetKey(string hullName)
        {
            Assert.IsTrue(_hullNameToKey.ContainsKey(hullName), hullName + " has no entry");
            return _hullNameToKey[hullName];
        }

        public HullKey GetKeyFromHullType(string hullType)
        {
            Assert.IsTrue(_hullTypeToKey.ContainsKey(hullType), hullType + " has no entry");
            return _hullTypeToKey[hullType];
        }
    }
}