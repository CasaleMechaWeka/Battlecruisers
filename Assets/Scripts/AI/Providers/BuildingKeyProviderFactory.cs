﻿using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers
{
    public class BuildingKeyProviderFactory : IBuildingKeyProviderFactory
    {
        private readonly IStaticData _staticData;

        public BuildingKeyProviderFactory(IStaticData staticData)
        {
            Assert.IsNotNull(staticData);
            _staticData = staticData;
        }

        public IBuildingKeyProvider CreateBuildingKeyProvider(BuildingCategory buildingCategory, int levelNum)
        {
            return new BuildingKeyProvider(_staticData, buildingCategory, levelNum);
        }

        public IBuildingKeyProvider CreateDummyBuildingKeyProvider(IPrefabKey buildingKey)
        {
            return new DummyBuildingKeyProvider(buildingKey);
        }
	}
}
