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
        private readonly IDictionary<string, BuildingKey> _buildingKey;
        private readonly IDictionary<string, UnitKey> _unitKey;

        public HullNameToKey(IList<HullKey> keys, IPrefabFactory prefabFactory)
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