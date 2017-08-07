﻿using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.AI.Providers.Strategies;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Providers
{
    public class BuildOrderProvider : IBuildOrderProvider
    {
        private readonly IBuildingKeyProviderFactory _buildingKeyProviderFactory;
        private readonly IOffensiveBuildOrderProvider _offensiveBuildOrderProvider;
        private readonly IStaticData _staticData;

        // FELIX  Use StaticData instead
		private const int LEVEL_SAM_SITE_IS_UNLOCKED = 2;
		private const int LEVEL_MORTAR_IS_UNLOCKED = 1;
		private const int LEVEL_TESLA_COIL_IS_UNLOCKED = 12;

        public IList<IPrefabKey> AntiRocketBuildOrder { get { return StaticBuildOrders.AntiRocketLauncher; } }

        public BuildOrderProvider(IBuildingKeyProviderFactory buildingKeyProviderFactory, 
            IOffensiveBuildOrderProvider offensiveBuildOrderProvider, IStaticData staticData)
        {
            Helper.AssertIsNotNull(buildingKeyProviderFactory, offensiveBuildOrderProvider, staticData);

            _buildingKeyProviderFactory = buildingKeyProviderFactory;
            _offensiveBuildOrderProvider = offensiveBuildOrderProvider;
            _staticData = staticData;
        }

        // FELIX  Avoid duplicate code?
        /// <summary>
        /// Gets the basic build order, which contains threat counters.
        /// </summary>
        public IList<IPrefabKey> GetBasicBuildOrder(int levelNum)
        {
            // Create offensive build order

            // Create base build order

            // FELIX  Don't always return same build order :P
            // FELIX  TEMP
            //return StaticBuildOrders.Basic.Balanced;
            return new List<IPrefabKey>();
        }
		
        /// <summary>
        /// Build orders do NOT contain threat counters.  These counters
        /// get created on the fly in response to threats.
        /// </summary>
        public IList<IPrefabKey> GetAdaptiveBuildOrder(int levelNum, int numOfPlatformSlots)
        {
            IStrategy strategy = _staticData.GetAdaptiveStrategy(levelNum);

            // Convert IBasicOffensiveRequests to IOffensiveRequests
            IList<IOffensiveRequest> offensiveRequests = strategy.Offensives.Select(basicRequest => 
            {
                IBuildingKeyProvider buildingKeyProvider = _buildingKeyProviderFactory.CreateBuildingKeyProvider(basicRequest.Type, levelNum);
                return (IOffensiveRequest)new OffensiveRequest(basicRequest.Type, basicRequest.Focus, buildingKeyProvider);
            }).ToList();

            // Create offensive build order
            IList<IPrefabKey> offensiveBuildOrder = _offensiveBuildOrderProvider.CreateBuildOrder(numOfPlatformSlots, offensiveRequests);
			IBuildOrders buildOrders = new BuildOrders(offensiveBuildOrder);

            IList<IPrefabKeyWrapper> baseBuildOrder = strategy.BaseStrategy.BuildOrder;

            // Initialise key wrappers, to offensive blanks to be filled
            foreach (IPrefabKeyWrapper keyWrapper in baseBuildOrder)
            {
                keyWrapper.Initialise(buildOrders);
            }

            return 
                baseBuildOrder
                    .Where(keyWrapper => keyWrapper.HasKey)
                    .Select(keyWrapper => keyWrapper.Key)
                    .ToList();
        }

        public IList<IPrefabKey> GetAntiAirBuildOrder(int levelNum)
        {
            return levelNum > LEVEL_SAM_SITE_IS_UNLOCKED ? StaticBuildOrders.AntiAir : StaticBuildOrders.BasicAntiAir;
        }

        public IList<IPrefabKey> GetAntiNavalBuildOrder(int levelNum)
        {
            return levelNum > LEVEL_MORTAR_IS_UNLOCKED ? StaticBuildOrders.AntiNaval : StaticBuildOrders.BasicAntiNaval;
        }

        public bool IsAntiRocketBuildOrderAvailable(int levelNum)
        {
            return levelNum > LEVEL_TESLA_COIL_IS_UNLOCKED;
        }
    }
}
