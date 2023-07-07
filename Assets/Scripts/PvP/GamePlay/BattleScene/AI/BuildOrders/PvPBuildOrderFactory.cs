using BattleCruisers.AI;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Requests;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public class PvPBuildOrderFactory : IPvPBuildOrderFactory
    {
        private readonly IPvPSlotAssigner _slotAssigner;
        private readonly IStaticData _staticData;
        private readonly IGameModel _gameModel;
        private readonly IPvPStrategyFactory _strategyFactory;

        private const int NUM_OF_NAVAL_FACTORY_SLOTS = 1;
        // For spy satellite launcher
        private const int NUM_OF_DECK_SLOTS_TO_RESERVE = 1;

        public PvPBuildOrderFactory(IPvPSlotAssigner slotAssigner, IStaticData staticData, IGameModel gameModel, IPvPStrategyFactory strategyFactory)
        {
            PvPHelper.AssertIsNotNull(slotAssigner, staticData, gameModel, strategyFactory);

            _slotAssigner = slotAssigner;
            _staticData = staticData;
            _gameModel = gameModel;
            _strategyFactory = strategyFactory;
        }

        /// <summary>
        /// Gets the basic build order, which contains counters to threats.
        /// </summary>
        public IPvPDynamicBuildOrder CreateBasicBuildOrder(ILevelInfo levelInfo)
        {
            IPvPStrategy strategy = _strategyFactory.GetBasicStrategy();
            return GetBuildOrder(strategy, levelInfo, hasDefensivePlaceholders: true);
        }

        /// <summary>
        /// Build orders do NOT contain counters to threats.  These counters
        /// get created on the fly in response to threats.
        /// </summary>
        public IPvPDynamicBuildOrder CreateAdaptiveBuildOrder(ILevelInfo levelInfo)
        {
            IPvPStrategy strategy = _strategyFactory.GetAdaptiveStrategy();
            return GetBuildOrder(strategy, levelInfo, hasDefensivePlaceholders: false);
        }

        private IPvPDynamicBuildOrder GetBuildOrder(IPvPStrategy strategy, ILevelInfo levelInfo, bool hasDefensivePlaceholders)
        {
            // Create offensive build order
            int numOfPlatformSlots = levelInfo.AICruiser.SlotAccessor.GetSlotCount(PvPSlotType.Platform);
            IPvPDynamicBuildOrder offensiveBuildOrder = CreateOffensiveBuildOrder(strategy.Offensives.ToList(), numOfPlatformSlots, levelInfo);

            // Create defensive build orders (only for basic AI)
            IPvPDynamicBuildOrder antiAirBuildOrder = hasDefensivePlaceholders ? CreateAntiAirBuildOrder(levelInfo) : null;
            IPvPDynamicBuildOrder antiNavalBuildOrder = hasDefensivePlaceholders ? CreateAntiNavalBuildOrder(levelInfo) : null;

            IPvPBuildOrders buildOrders = new PvPBuildOrders(offensiveBuildOrder, antiAirBuildOrder, antiNavalBuildOrder);

            IList<IPvPPrefabKeyWrapper> baseBuildOrder = strategy.BaseStrategy.BuildOrder;

            foreach (IPvPPrefabKeyWrapper keyWrapper in baseBuildOrder)
            {
                keyWrapper.Initialise(buildOrders);
            }

            return new PvPStrategyBuildOrder(baseBuildOrder.GetEnumerator(), levelInfo);
        }

        /// <summary>
        /// NOTE:  Must use IList as a parateter instead if IEnumerable.  Initially I used
        /// IEnumerable from a LINQ Select query, but every time I looped through this
        /// IEnumerable I would get a fresh copy of the object, so any changes I made to
        /// those objects were lost!!!
        /// </summary>
        private IPvPDynamicBuildOrder CreateOffensiveBuildOrder(IList<IPvPOffensiveRequest> requests, int numOfPlatformSlots, ILevelInfo levelInfo)
        {
            AssignSlots(_slotAssigner, requests, numOfPlatformSlots);

            // Create individual build orders
            IList<IPvPDynamicBuildOrder> buildOrders = new List<IPvPDynamicBuildOrder>();
            foreach (IPvPOffensiveRequest request in requests)
            {
                buildOrders.Add(CreateBuildOrder(request, levelInfo));
            }

            // Create combined build order
            return new PvPCombinedBuildOrders(buildOrders);
        }

        private void AssignSlots(IPvPSlotAssigner slotAssigner, IList<IPvPOffensiveRequest> requests, int numOfPlatformSlots)
        {
            // Should have a single naval request at most
            IPvPOffensiveRequest navalRequest = requests.FirstOrDefault(request => request.Type == PvPOffensiveType.Naval);
            if (navalRequest != null)
            {
                navalRequest.NumOfSlotsToUse = NUM_OF_NAVAL_FACTORY_SLOTS;
            }

            // All non-naval requests (offensives or non-banned ultras) require platform slots, 
            // so need to split the available platform slots between these requests.
            IEnumerable<IPvPOffensiveRequest> platformRequests = requests.Where(request => request.Type != PvPOffensiveType.Naval);
            slotAssigner.AssignSlots(platformRequests, numOfPlatformSlots);
        }

        private IPvPDynamicBuildOrder CreateBuildOrder(IPvPOffensiveRequest request, ILevelInfo levelInfo)
        {
            //Logging.Log(Tags.AI_BUILD_ORDERS, request.ToString());

            switch (request.Type)
            {
                case PvPOffensiveType.Air:
                    return CreateStaticBuildOrder(PvPStaticPrefabKeys.PvPBuildings.PvPAirFactory, request.NumOfSlotsToUse);

                case PvPOffensiveType.Naval:
                    return CreateStaticBuildOrder(PvPStaticPrefabKeys.PvPBuildings.PvPNavalFactory, request.NumOfSlotsToUse);

                case PvPOffensiveType.Buildings:
                    return CreateDynamicBuildOrder(PvPBuildingCategory.Offence, request.NumOfSlotsToUse, levelInfo);

                case PvPOffensiveType.Ultras:
                    return CreateDynamicBuildOrder(PvPBuildingCategory.Ultra, request.NumOfSlotsToUse, levelInfo, _staticData.AIBannedUltrakeys);

                default:
                    throw new ArgumentException();
            }
        }

        public IPvPDynamicBuildOrder CreateAntiAirBuildOrder(ILevelInfo levelInfo)
        {
            int numOfDeckSlots = levelInfo.AICruiser.SlotAccessor.GetSlotCount(PvPSlotType.Deck);
            int numOfSlotsToUse = PvPHelper.Half(numOfDeckSlots - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: true);

            return
                new PvPAntiUnitBuildOrder(
                    basicDefenceKey: PvPStaticPrefabKeys.PvPBuildings.PvPAntiAirTurret,
                    advancedDefenceKey: PvPStaticPrefabKeys.PvPBuildings.PvPSamSite,
                    levelInfo: levelInfo,
                    numOfSlotsToUse: numOfSlotsToUse);
        }

        public IPvPDynamicBuildOrder CreateAntiNavalBuildOrder(ILevelInfo levelInfo)
        {
            int numOfDeckSlots = levelInfo.AICruiser.SlotAccessor.GetSlotCount(PvPSlotType.Deck);
            int numOfSlotsToUse = PvPHelper.Half(numOfDeckSlots - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: false);

            return
                new PvPAntiUnitBuildOrder(
                    basicDefenceKey: PvPStaticPrefabKeys.PvPBuildings.PvPAntiShipTurret,
                    advancedDefenceKey: PvPStaticPrefabKeys.PvPBuildings.PvPMortar,
                    levelInfo: levelInfo,
                    numOfSlotsToUse: numOfSlotsToUse);
        }

        public bool IsAntiRocketBuildOrderAvailable()
        {
            return _gameModel.IsBuildingUnlocked(PvPStaticPrefabKeys.PvPBuildings.PvPTeslaCoil);
        }

        public IPvPDynamicBuildOrder CreateAntiRocketBuildOrder()
        {
            return CreateStaticBuildOrder(PvPStaticPrefabKeys.PvPBuildings.PvPTeslaCoil, size: 1);
        }

        public bool IsAntiStealthBuildOrderAvailable()
        {
            return _gameModel.IsBuildingUnlocked(PvPStaticPrefabKeys.PvPBuildings.PvPSpySatelliteLauncher);
        }

        public IPvPDynamicBuildOrder CreateAntiStealthBuildOrder()
        {
            return CreateStaticBuildOrder(PvPStaticPrefabKeys.PvPBuildings.PvPSpySatelliteLauncher, size: 1);
        }

        private IPvPDynamicBuildOrder CreateStaticBuildOrder(PvPBuildingKey buildingKey, int size)
        {
            return
                new PvPFiniteBuildOrder(
                    new PvPInfiniteStaticBuildOrder(buildingKey),
                    size);
        }

        private IPvPDynamicBuildOrder CreateDynamicBuildOrder(
            PvPBuildingCategory buildingCategory,
            int size,
            ILevelInfo levelInfo,
            IList<PvPBuildingKey> bannedBuildings = null)
        {
            return
                new PvPFiniteBuildOrder(
                    new PvPInfiniteBuildOrder(buildingCategory, levelInfo, bannedBuildings),
                    size);
        }
    }
}
