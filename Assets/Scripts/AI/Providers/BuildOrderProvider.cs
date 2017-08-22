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
    // FELIX  Delete
    public class BuildOrderProvider : IBuildOrderProvider
    {
        private readonly IBuildingKeyProviderFactory _buildingKeyProviderFactory;
        private readonly IOffensiveBuildOrderProvider _offensiveBuildOrderProvider;
        private readonly IAntiUnitBuildOrderProvider _antiAirBuildOrderProvider, _antiNavalBuildOrderProvider;
        private readonly IStaticData _staticData;

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

        /// <summary>
        /// Gets the basic build order, which contains counters to threats.
        /// </summary>
        public IList<IPrefabKey> GetBasicBuildOrder(int levelNum, ISlotWrapper slotWrapper)
        {
            IStrategy strategy = _staticData.GetBasicStrategy(levelNum);
            return GetBuildOrder(strategy, levelNum, slotWrapper, hasDefensivePlaceholders: true);
        }

		/// <summary>
		/// Build orders do NOT contain counters to threats.  These counters
		/// get created on the fly in response to threats.
		/// </summary>
		public IList<IPrefabKey> GetAdaptiveBuildOrder(int levelNum, ISlotWrapper slotWrapper)
		{
			IStrategy strategy = _staticData.GetAdaptiveStrategy(levelNum);
			return GetBuildOrder(strategy, levelNum, slotWrapper, hasDefensivePlaceholders: false);
		}

        private IList<IPrefabKey> GetBuildOrder(IStrategy strategy, int levelNum, ISlotWrapper slotWrapper, bool hasDefensivePlaceholders)
        {
			// Convert IBasicOffensiveRequests to IOffensiveRequests
			IList<IOffensiveRequest> offensiveRequests = strategy.Offensives.Select(basicRequest =>
			{
				IBuildingKeyProvider buildingKeyProvider = _buildingKeyProviderFactory.CreateBuildingKeyProvider(basicRequest.Type, levelNum);
				return (IOffensiveRequest)new OffensiveRequest(basicRequest, buildingKeyProvider);
			}).ToList();

            // Create offensive build order
            int numOfPlatformSlots = slotWrapper.GetSlotCount(SlotType.Platform);
			IList<IPrefabKey> offensiveBuildOrder = _offensiveBuildOrderProvider.CreateBuildOrder(numOfPlatformSlots, offensiveRequests);

            // Create defensive build orders
            int numOfDeckSlots = slotWrapper.GetSlotCount(SlotType.Deck);
            IList<IPrefabKey> antiAirBuildOrder = hasDefensivePlaceholders ? _antiAirBuildOrderProvider.CreateBuildOrder(numOfDeckSlots, levelNum) : new List<IPrefabKey>();
            IList<IPrefabKey> antiNavalBuildOrder = hasDefensivePlaceholders ? _antiNavalBuildOrderProvider.CreateBuildOrder(numOfDeckSlots, levelNum) : new List<IPrefabKey>();

            //IBuildOrders buildOrders = new BuildOrders(offensiveBuildOrder, antiAirBuildOrder, antiNavalBuildOrder);

			//IList<IPrefabKeyWrapper> baseBuildOrder = strategy.BaseStrategy.BuildOrder;

			//// Initialise key wrappers, so offensive and defensive placeholders are filled
			//foreach (IPrefabKeyWrapper keyWrapper in baseBuildOrder)
			//{
			//	keyWrapper.Initialise(buildOrders);
			//}

			//return
				//baseBuildOrder
					//.Where(keyWrapper => keyWrapper.HasKey)
					//.Select(keyWrapper => keyWrapper.Key)
     //               .Where(key => _staticData.IsBuildableAvailable(key, levelNum))
					//.ToList();

            // FELIX
            return null;
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
            return _staticData.IsBuildableAvailable(StaticPrefabKeys.Buildings.TeslaCoil, levelNum);
        }
    }
}
