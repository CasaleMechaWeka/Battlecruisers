using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Data.Static.Strategies.Requests;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.AI.BuildOrders
{
    public class BuildOrderFactory
    {
        private readonly SlotAssigner _slotAssigner;
        private readonly IStrategyFactory _strategyFactory;

        private const int NUM_OF_NAVAL_FACTORY_SLOTS = 1;
        private const int NUM_OF_AIR_FACTORY_SLOTS_TO_RESERVE = 1;
        // For spy satellite launcher
        private const int NUM_OF_DECK_SLOTS_TO_RESERVE = 1;
        private int numOfMastSlotsToReserve = 2;    // not constant because we don't always want to build a StealthGen

        List<HullType> goodStealthCruisers = new List<HullType>
        {
            HullType.Trident,
            HullType.Rickshaw
        };

        public BuildOrderFactory(SlotAssigner slotAssigner, IStrategyFactory strategyFactory)
        {
            Helper.AssertIsNotNull(slotAssigner, strategyFactory);

            _slotAssigner = slotAssigner;
            _strategyFactory = strategyFactory;
        }

        /// <summary>
        /// Gets the basic build order, which contains counters to threats.
        /// </summary>

        /// <summary>
        /// Build orders do NOT contain counters to threats.  These counters
        /// get created on the fly in response to threats.
        /// </summary>
        public BuildingKey[] CreateAdaptiveBuildOrder(LevelInfo levelInfo)
        {
            Strategy strategy = _strategyFactory.GetAdaptiveStrategy();

            // we do not want the AI to build a StealthGen when they only have 1 mast because that makes
            // it very vulnerable to MissileRevolver
            if (DataProvider.SettingsManager.AIDifficulty == Data.Settings.Difficulty.Harder
                && (levelInfo.AICruiser.SlotNumProvider.GetSlotCount(SlotType.Mast) <= 1)
                    || (goodStealthCruisers.Contains(levelInfo.PlayerCruiser.hullType)
                        && !goodStealthCruisers.Contains(levelInfo.AICruiser.hullType)))
            {
                for (int i = 0; i < strategy.BaseStrategy.Count; i++)
                    if (strategy.BaseStrategy[i] == StaticPrefabKeys.Buildings.StealthGenerator)
                        strategy.BaseStrategy.RemoveAt(i);
            }

            if (!strategy.BaseStrategy.Contains(StaticPrefabKeys.Buildings.StealthGenerator))
                numOfMastSlotsToReserve--;

            for (int i = 0; i < strategy.BaseStrategy.Count; i++)
                if (strategy.BaseStrategy[i] != null && !DataProvider.GameModel.IsBuildingUnlocked(strategy.BaseStrategy[i]))
                {
                    strategy.BaseStrategy.RemoveAt(i);
                    i--;
                }

            return GetBuildOrder(strategy, levelInfo);
        }

        private BuildingKey[] GetBuildOrder(Strategy strategy, LevelInfo levelInfo)
        {
            // Create offensive build order
            //int numOfPlatformSlots = levelInfo.AICruiser.SlotAccessor.GetSlotCount(SlotType.Platform);

            int numOfOffensiveSlots = FindNumOfOffensiveSlots(levelInfo);

            // What do we need?
            // SlotAccessor for the slot count (Platform, Bow and Mast slots, anything that can host offensives)                   

            List<BuildingKey> offensiveBuildOrder = CreateOffensiveBuildOrder(strategy.Offensives.ToList(), numOfOffensiveSlots).ToList();
            Debug.Log(offensiveBuildOrder.Count + " " + strategy.Offensives);
            for (int i = 0; i < strategy.BaseStrategy.Count; i++)
                if (strategy.BaseStrategy[i] == null)
                {
                    if (offensiveBuildOrder.Count > 0)
                    {
                        strategy.BaseStrategy[i] = offensiveBuildOrder[0];
                        offensiveBuildOrder.RemoveAt(0);
                    }
                }

            offensiveBuildOrder = offensiveBuildOrder.Where(x => x != null).ToList();
            for (int i = 0; i < offensiveBuildOrder.Count; i++)
                if (offensiveBuildOrder[i] == null)
                    Debug.Log("RAAAAA");

            return strategy.BaseStrategy.ToArray();
        }

        // TODO: Find a better class to move this to. Make the method public to add unit test!
        private int FindNumOfOffensiveSlots(LevelInfo levelInfo)
        {
            SlotAccessor slotAccessor = levelInfo.AICruiser.SlotAccessor;
            int numOfOffensiveSlots = slotAccessor.GetSlotCount(SlotType.Platform);

            if (levelInfo.HasSlotType(SlotType.Mast))
                numOfOffensiveSlots += slotAccessor.GetSlotCount(SlotType.Mast) - numOfMastSlotsToReserve;

            if (levelInfo.HasSlotType(SlotType.Bow))
                numOfOffensiveSlots += slotAccessor.GetSlotCount(SlotType.Bow);

            return numOfOffensiveSlots;
        }

        private BuildingKey[] CreateOffensiveBuildOrder(IList<OffensiveRequest> requests, int numOfPlatformSlots)
        {
            AssignSlots(_slotAssigner, requests, numOfPlatformSlots);

            // Create individual build orders
            List<BuildingKey> buildOrders = new List<BuildingKey>();
            foreach (OffensiveRequest request in requests)
                buildOrders.AddRange(CreateBuildOrder(request));

            return buildOrders.ToArray();
        }

        private void AssignSlots(SlotAssigner slotAssigner, IList<OffensiveRequest> requests, int numOfPlatformSlots)
        {
            // Should have a single naval request at most
            OffensiveRequest navalRequest = requests.FirstOrDefault(request => request.Type == OffensiveType.Naval);
            if (navalRequest != null)
            {
                navalRequest.NumOfSlotsToUse = NUM_OF_NAVAL_FACTORY_SLOTS;
            }

            // Should have a single air request at most
            OffensiveRequest airRequest = requests.FirstOrDefault(request => request.Type == OffensiveType.Air);
            if (airRequest != null)
            {
                airRequest.NumOfSlotsToUse = NUM_OF_AIR_FACTORY_SLOTS_TO_RESERVE;
            }

            // All non-naval requests (offensives or non-banned ultras) require platform slots, 
            // so need to split the available platform slots between these requests.
            IEnumerable<OffensiveRequest> platformRequests = requests.Where
                (request => request.Type != OffensiveType.Naval && request.Type != OffensiveType.Air);
            slotAssigner.AssignSlots(platformRequests, numOfPlatformSlots);
        }

        private BuildingKey[] CreateBuildOrder(OffensiveRequest request)
        {
            Logging.Log(Tags.AI_BUILD_ORDERS, request.ToString());

            return request.Type switch
            {
                OffensiveType.Air => CreateStaticBuildOrder(StaticPrefabKeys.Buildings.AirFactory, request.NumOfSlotsToUse),
                OffensiveType.Naval => CreateStaticBuildOrder(StaticPrefabKeys.Buildings.NavalFactory, request.NumOfSlotsToUse),
                OffensiveType.Buildings => CreateDynamicBuildOrder(BuildingCategory.Offence, request.NumOfSlotsToUse),
                OffensiveType.Ultras => CreateDynamicBuildOrder(BuildingCategory.Ultra, request.NumOfSlotsToUse, StaticData.AIBannedUltrakeys),
                _ => throw new ArgumentException(),
            };
        }

        public BuildingKey[] CreateAntiAirBuildOrder(LevelInfo levelInfo)
        {
            int numOfDeckSlots = levelInfo.AICruiser.SlotAccessor.GetSlotCount(SlotType.Deck);
            int numOfSlotsToUse = Helper.Half(numOfDeckSlots - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: true);

            BuildingKey[] buildOrder = new BuildingKey[numOfSlotsToUse];
            buildOrder[0] = StaticPrefabKeys.Buildings.AntiAirTurret;

            BuildingKey filler = DataProvider.GameModel.IsBuildingUnlocked(StaticPrefabKeys.Buildings.SamSite) ?
                                 StaticPrefabKeys.Buildings.SamSite :
                                 StaticPrefabKeys.Buildings.AntiAirTurret;

            for (int i = 1; i < buildOrder.Length; i++)
                buildOrder[i] = filler;

            return buildOrder;
        }

        public BuildingKey[] CreateAntiNavalBuildOrder(LevelInfo levelInfo)
        {
            int numOfDeckSlots = levelInfo.AICruiser.SlotAccessor.GetSlotCount(SlotType.Deck);
            int numOfSlotsToUse = Helper.Half(numOfDeckSlots - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: false);

            BuildingKey[] buildOrder = new BuildingKey[numOfSlotsToUse];
            buildOrder[0] = StaticPrefabKeys.Buildings.AntiShipTurret;

            BuildingKey filler = DataProvider.GameModel.IsBuildingUnlocked(StaticPrefabKeys.Buildings.Mortar) ?
                                 StaticPrefabKeys.Buildings.Mortar :
                                 StaticPrefabKeys.Buildings.AntiShipTurret;

            for (int i = 1; i < buildOrder.Length; i++)
                buildOrder[i] = filler;

            return buildOrder;
        }

        private BuildingKey[] CreateStaticBuildOrder(BuildingKey buildingKey, int size)
        {
            BuildingKey[] buildOrder = new BuildingKey[size];
            for (int i = 0; i < buildOrder.Length; i++)
                buildOrder[i] = buildingKey;
            return buildOrder;
        }

        private BuildingKey[] CreateDynamicBuildOrder(
            BuildingCategory buildingCategory,
            int size,
            IList<BuildingKey> bannedBuildings = null)
        {
            List<BuildingKey> buildOrder = new List<BuildingKey>();
            List<BuildingKey> availableBuildings = DataProvider.GameModel.GetUnlockedBuildings(buildingCategory);

            if (bannedBuildings != null)
                foreach (BuildingKey key in bannedBuildings)
                    availableBuildings.Remove(key);

            for (int i = 0; i < size; i++)
            {
                buildOrder.Add(availableBuildings
                                .Where(DataProvider.GameModel.IsBuildingUnlocked)
                                .Shuffle()
                                .FirstOrDefault());
            }

            return buildOrder.ToArray();
        }
    }
}
