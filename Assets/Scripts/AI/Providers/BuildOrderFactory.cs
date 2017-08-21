using System;
using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.AI.Providers.Strategies.Requests;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers
{
    public class BuildOrderFactory : IBuildOrderFactory
	{
        private readonly IBuildingKeyHelper _buildingKeyHelper;

        public BuildOrderFactory(IBuildingKeyHelper buildingKeyHelper)
		{
            Assert.IsNotNull(buildingKeyHelper);
            _buildingKeyHelper = buildingKeyHelper;
		}

        public IDynamicBuildOrder CreateBuildOrder(IOffensiveRequest request)
        {
            switch (request.Type)
			{
				case OffensiveType.Air:
                    return CreateStaticBuildOrder(StaticPrefabKeys.Buildings.AirFactory, request.NumOfSlotsToUse);
					
				case OffensiveType.Naval:
                    return CreateStaticBuildOrder(StaticPrefabKeys.Buildings.NavalFactory, request.NumOfSlotsToUse);
					
				case OffensiveType.Buildings:
                    return CreateDynamicBuildOrder(BuildingCategory.Offence, request.NumOfSlotsToUse);
					
				case OffensiveType.Ultras:
                    return CreateDynamicBuildOrder(BuildingCategory.Ultra, request.NumOfSlotsToUse);
					
				default:
					throw new ArgumentException();
			}
        }

        private IDynamicBuildOrder CreateStaticBuildOrder(IPrefabKey buildingKey, int size)
        {
            return
                new FiniteBuildOrder(
                    new InfiniteStaticBuildOrder(buildingKey),
                    size);
        }

        private IDynamicBuildOrder CreateDynamicBuildOrder(BuildingCategory buildingCategory, int size)
        {
            return
                new FiniteBuildOrder(
                    new InfiniteBuildOrder(buildingCategory, _buildingKeyHelper),
                    size);
        }
    }
}
