using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Data.Static.Strategies.Requests;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Providers
{
    public class BuildOrderFactory : IBuildOrderFactory
	{
        private readonly ISlotAssigner _slotAssigner;
        private readonly IStaticData _staticData;
		
        private const int NUM_OF_NAVAL_FACTORY_SLOTS = 1;

		public BuildOrderFactory(ISlotAssigner slotAssigner, IStaticData staticData)
		{
            Helper.AssertIsNotNull(slotAssigner, staticData);

            _slotAssigner = slotAssigner;
            _staticData = staticData;
		}

		/// <summary>
		/// Gets the basic build order, which contains counters to threats.
		/// </summary>
        public IDynamicBuildOrder CreateBasicBuildOrder(ILevelInfo levelInfo)
		{
            IStrategy strategy = _staticData.GetBasicStrategy(levelInfo.LevelNum);
            return GetBuildOrder(strategy, levelInfo, hasDefensivePlaceholders: true);
		}

		/// <summary>
		/// Build orders do NOT contain counters to threats.  These counters
		/// get created on the fly in response to threats.
		/// </summary>
        public IDynamicBuildOrder CreateAdaptiveBuildOrder(ILevelInfo levelInfo)
		{
            IStrategy strategy = _staticData.GetAdaptiveStrategy(levelInfo.LevelNum);
            return GetBuildOrder(strategy, levelInfo, hasDefensivePlaceholders: false);
		}

        private IDynamicBuildOrder GetBuildOrder(IStrategy strategy, ILevelInfo levelInfo, bool hasDefensivePlaceholders)
		{
			// Create offensive build order
            int numOfPlatformSlots = levelInfo.AICruiser.SlotWrapper.GetSlotCount(SlotType.Platform);
            IDynamicBuildOrder offensiveBuildOrder = CreateOffensiveBuildOrder(strategy.Offensives.ToList(), numOfPlatformSlots, levelInfo);

            // Create defensive build orders (only for basic AI)
            IDynamicBuildOrder antiAirBuildOrder = hasDefensivePlaceholders ? CreateAntiAirBuildOrder(levelInfo) : null;
            IDynamicBuildOrder antiNavalBuildOrder = hasDefensivePlaceholders ? CreateAntiNavalBuildOrder(levelInfo) : null;

			IBuildOrders buildOrders = new BuildOrders(offensiveBuildOrder, antiAirBuildOrder, antiNavalBuildOrder);

			IList<IPrefabKeyWrapper> baseBuildOrder = strategy.BaseStrategy.BuildOrder;

            foreach (IPrefabKeyWrapper keyWrapper in baseBuildOrder)
			{
				keyWrapper.Initialise(buildOrders);
			}

            return new StrategyBuildOrder(baseBuildOrder);
		}

		/// <summary>
		/// NOTE:  Must use IList as a parateter instead if IEnumerable.  Initially I used
		/// IEnumerable from a LINQ Select query, but every time I looped through this
		/// IEnumerable I would get a fresh copy of the object, so any changes I made to
		/// those objects were lost!!!
		/// </summary>
        private IDynamicBuildOrder CreateOffensiveBuildOrder(IList<IOffensiveRequest> requests, int numOfPlatformSlots, ILevelInfo levelInfo)
		{
			AssignSlots(_slotAssigner, requests, numOfPlatformSlots);

			// Create individual build orders
			IList<IDynamicBuildOrder> buildOrders = new List<IDynamicBuildOrder>();
			foreach (IOffensiveRequest request in requests)
			{
                buildOrders.Add(CreateBuildOrder(request, levelInfo));
			}

			// Create combined build order
			return new CombinedBuildOrders(buildOrders);
		}

		private void AssignSlots(ISlotAssigner slotAssigner, IList<IOffensiveRequest> requests, int numOfPlatformSlots)
		{
			// Should have a single naval request at most
			IOffensiveRequest navalRequest = requests.FirstOrDefault(request => request.Type == OffensiveType.Naval);
			if (navalRequest != null)
			{
				navalRequest.NumOfSlotsToUse = NUM_OF_NAVAL_FACTORY_SLOTS;
			}

			// All non-naval requests require platform slots, so need to split the available
			// platform slots between these requests.
			// FELIX  Handle ArchonBattleship Ultra, which may be the exception :P
			IEnumerable<IOffensiveRequest> platformRequests = requests.Where(request => request.Type != OffensiveType.Naval);
            slotAssigner.AssignSlots(platformRequests, numOfPlatformSlots);
		}

        private IDynamicBuildOrder CreateBuildOrder(IOffensiveRequest request, ILevelInfo levelInfo)
        {
            switch (request.Type)
			{
				case OffensiveType.Air:
                    return CreateStaticBuildOrder(StaticPrefabKeys.Buildings.AirFactory, request.NumOfSlotsToUse);
					
				case OffensiveType.Naval:
                    return CreateStaticBuildOrder(StaticPrefabKeys.Buildings.NavalFactory, request.NumOfSlotsToUse);
					
				case OffensiveType.Buildings:
                    return CreateDynamicBuildOrder(BuildingCategory.Offence, request.NumOfSlotsToUse, levelInfo);
					
				case OffensiveType.Ultras:
                    return CreateDynamicBuildOrder(BuildingCategory.Ultra, request.NumOfSlotsToUse, levelInfo);
					
				default:
					throw new ArgumentException();
			}
        }

        public IDynamicBuildOrder CreateAntiAirBuildOrder(ILevelInfo levelInfo)
        {
            int numOfDeckSlots = levelInfo.AICruiser.SlotWrapper.GetSlotCount(SlotType.Deck);
			int numOfSlotsToUse = Helper.Half(numOfDeckSlots, roundUp: true);

            return
                new AntiUnitBuildOrder(
                    basicDefenceKey: StaticPrefabKeys.Buildings.AntiAirTurret,
                    advancedDefenceKey: StaticPrefabKeys.Buildings.SamSite,
                    levelInfo: levelInfo,
                    numOfSlotsToUse: numOfSlotsToUse);
        }

		public IDynamicBuildOrder CreateAntiNavalBuildOrder(ILevelInfo levelInfo)
		{
            int numOfDeckSlots = levelInfo.AICruiser.SlotWrapper.GetSlotCount(SlotType.Deck);
			int numOfSlotsToUse = Helper.Half(numOfDeckSlots, roundUp: false);

			return
				new AntiUnitBuildOrder(
                    basicDefenceKey: StaticPrefabKeys.Buildings.AntiShipTurret,
                    advancedDefenceKey: StaticPrefabKeys.Buildings.Mortar,
                    levelInfo: levelInfo,
					numOfSlotsToUse: numOfSlotsToUse);
		}
		
        public bool IsAntiRocketBuildOrderAvailable(int levelNum)
        {
            return _staticData.IsBuildableAvailable(StaticPrefabKeys.Buildings.TeslaCoil, levelNum);
        }

        public IDynamicBuildOrder CreateAntiRocketBuildOrder()
        {
            return CreateStaticBuildOrder(StaticPrefabKeys.Buildings.TeslaCoil, size: 1);
        }

		private IDynamicBuildOrder CreateStaticBuildOrder(IPrefabKey buildingKey, int size)
		{
			return
				new FiniteBuildOrder(
					new InfiniteStaticBuildOrder(buildingKey),
					size);
		}

		private IDynamicBuildOrder CreateDynamicBuildOrder(BuildingCategory buildingCategory, int size, ILevelInfo levelInfo)
		{
			return
				new FiniteBuildOrder(
                    new InfiniteBuildOrder(buildingCategory, levelInfo),
					size);
		}
    }
}
