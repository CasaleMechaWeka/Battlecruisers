using System;
using BattleCruisers.AI.Providers.Strategies;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Static;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public class BuildingKeyProviderFactory : IBuildingKeyProviderFactory
    {
        private readonly IStaticData _staticData;

        public BuildingKeyProviderFactory(IStaticData staticData)
        {
            Assert.IsNotNull(staticData);
            _staticData = staticData;
        }

        public IBuildingKeyProvider CreateBuildingKeyProvider(OffensiveType offensiveType, int levelNum)
        {
            switch (offensiveType)
            {
                case OffensiveType.Air:
                    return new DummyBuildingKeyProvider(StaticPrefabKeys.Buildings.AirFactory);

                case OffensiveType.Naval:
                    return new DummyBuildingKeyProvider(StaticPrefabKeys.Buildings.NavalFactory);

                case OffensiveType.Buildings:
                    return new BuildingKeyProvider(_staticData, BuildingCategory.Offence, levelNum);

                case OffensiveType.Ultras:
                    return new BuildingKeyProvider(_staticData, BuildingCategory.Ultra, levelNum);

                default:
                    throw new ArgumentException();
            }
        }
	}
}
