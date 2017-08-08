using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.AI.Providers.Strategies;
using BattleCruisers.AI.Providers.Strategies.Requests;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Providers
{
    public class BuildOrderProvider : IBuildOrderProvider
    {
        private readonly IBuildingKeyProviderFactory _buildingKeyProviderFactory;
        private readonly IOffensiveBuildOrderProvider _offensiveBuildOrderProvider;
        private readonly IAntiUnitBuildOrderProvider _antiAirBuildOrderProvider, _antiNavalBuildOrderProvider;
        private readonly IStaticData _staticData;

        // FELIX  Use StaticData instead
		private const int LEVEL_TESLA_COIL_IS_UNLOCKED = 12;

        public IList<IPrefabKey> AntiRocketBuildOrder { get { return StaticBuildOrders.AntiRocketLauncher; } }

        public BuildOrderProvider(IBuildingKeyProviderFactory buildingKeyProviderFactory, IOffensiveBuildOrderProvider offensiveBuildOrderProvider, 
            IAntiUnitBuildOrderProvider antiAirBuildOrderProvider, IAntiUnitBuildOrderProvider antiNavalBuildOrderProvider, IStaticData staticData)
        {
            Helper.AssertIsNotNull(buildingKeyProviderFactory, offensiveBuildOrderProvider, antiAirBuildOrderProvider, antiNavalBuildOrderProvider, staticData);

            _buildingKeyProviderFactory = buildingKeyProviderFactory;
            _offensiveBuildOrderProvider = offensiveBuildOrderProvider;
            _antiAirBuildOrderProvider = antiAirBuildOrderProvider;
            _antiNavalBuildOrderProvider = antiNavalBuildOrderProvider;
            _staticData = staticData;
        }

        // FELIX  Avoid duplicate code?
        /// <summary>
        /// Gets the basic build order, which contains threat counters.
        /// </summary>
        public IList<IPrefabKey> GetBasicBuildOrder(int levelNum, ISlotWrapper slotWrapper)
        {
            IStrategy strategy = _staticData.GetBasicStrategy(levelNum);

			// FELIX  Avoid duplicate code
			// Convert IBasicOffensiveRequests to IOffensiveRequests
			IList<IOffensiveRequest> offensiveRequests = strategy.Offensives.Select(basicRequest =>
			{
				IBuildingKeyProvider buildingKeyProvider = _buildingKeyProviderFactory.CreateBuildingKeyProvider(basicRequest.Type, levelNum);
				return (IOffensiveRequest)new OffensiveRequest(basicRequest.Type, basicRequest.Focus, buildingKeyProvider);
			}).ToList();

            // Create offensive build order
            int numOfPlatformSlots = slotWrapper.GetSlotCount(SlotType.Platform);
			IList<IPrefabKey> offensiveBuildOrder = _offensiveBuildOrderProvider.CreateBuildOrder(numOfPlatformSlots, offensiveRequests);

            // Create defensive build orders
            int numOfDeckSlots = slotWrapper.GetSlotCount(SlotType.Deck);
            IList<IPrefabKey> antiAirBuildOrder = _antiAirBuildOrderProvider.CreateBuildOrder(numOfDeckSlots, levelNum);
            IList<IPrefabKey> antiNavalBuildOrder = _antiNavalBuildOrderProvider.CreateBuildOrder(numOfDeckSlots, levelNum);

            IBuildOrders buildOrders = new BuildOrders(offensiveBuildOrder, antiAirBuildOrder, antiNavalBuildOrder);

			IList<IPrefabKeyWrapper> baseBuildOrder = strategy.BaseStrategy.BuildOrder;

			// Initialise key wrappers, to offensive and defensive blanks to be filled
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
		
        /// <summary>
        /// Build orders do NOT contain counters to threats.  These counters
        /// get created on the fly in response to threats.
        /// </summary>
        public IList<IPrefabKey> GetAdaptiveBuildOrder(int levelNum, ISlotWrapper slotWrapper)
        {
            IStrategy strategy = _staticData.GetAdaptiveStrategy(levelNum);

            // Convert IBasicOffensiveRequests to IOffensiveRequests
            IList<IOffensiveRequest> offensiveRequests = strategy.Offensives.Select(basicRequest => 
            {
                IBuildingKeyProvider buildingKeyProvider = _buildingKeyProviderFactory.CreateBuildingKeyProvider(basicRequest.Type, levelNum);
                return (IOffensiveRequest)new OffensiveRequest(basicRequest.Type, basicRequest.Focus, buildingKeyProvider);
            }).ToList();

			// Create offensive build order
			int numOfPlatformSlots = slotWrapper.GetSlotCount(SlotType.Platform);
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

        public IList<IPrefabKey> GetAntiAirBuildOrder(int levelNum, ISlotWrapper slotWrapper)
        {
            int numOfDeckSlots = slotWrapper.GetSlotCount(SlotType.Deck);
            return _antiAirBuildOrderProvider.CreateBuildOrder(numOfDeckSlots, levelNum);
        }

        public IList<IPrefabKey> GetAntiNavalBuildOrder(int levelNum, ISlotWrapper slotWrapper)
        {
			int numOfDeckSlots = slotWrapper.GetSlotCount(SlotType.Deck);
			return _antiNavalBuildOrderProvider.CreateBuildOrder(numOfDeckSlots, levelNum);
        }

        public bool IsAntiRocketBuildOrderAvailable(int levelNum)
        {
            return levelNum > LEVEL_TESLA_COIL_IS_UNLOCKED;
        }
    }
}
