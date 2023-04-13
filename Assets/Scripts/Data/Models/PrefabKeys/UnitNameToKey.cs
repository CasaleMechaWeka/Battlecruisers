using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class UnitNameToKey : IUnitNameToKey
    {
        private readonly IDictionary<string, UnitKey> _unitNameToKey;

        public UnitNameToKey(IList<UnitKey> keys, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(keys, prefabFactory);

            _unitNameToKey = new Dictionary<string, UnitKey>();

            foreach (UnitKey key in keys)
            {
                IUnit unitPrefab = (IUnit)prefabFactory.GetUnitWrapperPrefab(key);
                _unitNameToKey.Add(unitPrefab.Name, key);
            }
        }

        public UnitKey GetKey(string hullName)
        {
            Assert.IsTrue(_unitNameToKey.ContainsKey(hullName));
            return _unitNameToKey[hullName];
        }
    }
}