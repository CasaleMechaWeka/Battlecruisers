using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class UnitNameToKey : IUnitNameToKey
    {
        private readonly IDictionary<string, UnitKey> _unitNameToKey;

        public UnitNameToKey(IList<UnitKey> keys)
        {
            Helper.AssertIsNotNull(keys);

            _unitNameToKey = new Dictionary<string, UnitKey>();

            foreach (UnitKey key in keys)
            {
                IBuildableWrapper<IUnit> unitPrefab = PrefabFactory.GetUnitWrapperPrefab(key);
                try
                {
                    _unitNameToKey.Add(unitPrefab.Buildable.Name, key);
                }
                catch
                {

                }
            }
        }

        public UnitKey GetKey(string hullName)
        {
            Assert.IsTrue(_unitNameToKey.ContainsKey(hullName));
            return _unitNameToKey[hullName];
        }
    }
}