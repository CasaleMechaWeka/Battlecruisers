using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class BuildingNameToKey : IBuildingNameToKey
    {
        private readonly IDictionary<string, BuildingKey> _buildingNameToKey;

        public BuildingNameToKey(IList<BuildingKey> keys, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(keys, prefabFactory);

            _buildingNameToKey = new Dictionary<string, BuildingKey>();

            foreach (BuildingKey key in keys)
            {
                IBuildable buildingPrefab = (IBuildable)prefabFactory.GetBuildingWrapperPrefab(key);
                _buildingNameToKey.Add(buildingPrefab.Name, key);
            }
        }

        public BuildingKey GetKey(string buildName)
        {
            Assert.IsTrue(_buildingNameToKey.ContainsKey(buildName));
            return _buildingNameToKey[buildName];
        }
    }
}