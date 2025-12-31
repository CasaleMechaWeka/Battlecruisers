using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class BuildingNameToKey : IBuildingNameToKey
    {
        private readonly IDictionary<string, BuildingKey> _buildingNameToKey;

        public BuildingNameToKey(IList<BuildingKey> keys)
        {
            Helper.AssertIsNotNull(keys);

            _buildingNameToKey = new Dictionary<string, BuildingKey>();

            foreach (BuildingKey key in keys)
            {
                IBuildableWrapper<IBuilding> buildingPrefab = PrefabFactory.GetBuildingWrapperPrefab(key);
                _buildingNameToKey.Add(buildingPrefab.Buildable.Name, key);
            }
        }

        public BuildingKey GetKey(string buildName)
        {
            Assert.IsTrue(_buildingNameToKey.ContainsKey(buildName));
            return _buildingNameToKey[buildName];
        }
    }
}