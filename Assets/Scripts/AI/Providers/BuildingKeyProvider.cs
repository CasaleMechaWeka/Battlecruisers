﻿using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers
{
    public class BuildingKeyProvider : IBuildingKeyProvider
    {
        private readonly ICircularList<IPrefabKey> _buildingKeys;

        public IPrefabKey Next { get { return _buildingKeys.Next(); } }

        public BuildingKeyProvider(IStaticData staticData, BuildingCategory buildingCategory, int levelNum)
        {
            IList<IPrefabKey> buildingKeys = staticData.GetAvailableBuildings(buildingCategory, levelNum);

            Assert.IsTrue(buildingKeys.Count != 0);

            buildingKeys.Shuffle();

            _buildingKeys = new CircularList<IPrefabKey>(buildingKeys);
        }
    }
}