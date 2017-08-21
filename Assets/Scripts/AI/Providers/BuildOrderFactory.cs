using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.AI.Providers.Strategies.Requests;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.Providers
{
    public class BuildOrderFactory : IBuildOrderFactory
	{
        private readonly IBuildingKeyHelper _buildingKeyHelper;
        private readonly ISlotAssigner _slotAssigner;
		
        private const int NUM_OF_NAVAL_FACTORY_SLOTS = 1;

        // FELIX  Do make IBuildingKeyHelper be tied to a level :(
		public BuildOrderFactory(IBuildingKeyHelper buildingKeyHelper, ISlotAssigner slotAssigner)
		{
            Helper.AssertIsNotNull(buildingKeyHelper, slotAssigner);

            _buildingKeyHelper = buildingKeyHelper;
            _slotAssigner = slotAssigner;
		}

		/// <summary>
		/// NOTE:  Must use IList as a parateter instead if IEnumerable.  Initially I used
		/// IEnumerable from a LINQ Select query, but every time I looped through this
		/// IEnumerable I would get a fresh copy of the object, so any changes I made to
		/// those objects were lost!!!
		/// </summary>
        public IDynamicBuildOrder CreateOffensiveBuildOrder(IList<IOffensiveRequest> requests, int numOfPlatformSlots)
		{
			AssignSlots(_slotAssigner, requests, numOfPlatformSlots);

			// Create individual build orders
			IList<IDynamicBuildOrder> buildOrders = new List<IDynamicBuildOrder>();
			foreach (IOffensiveRequest request in requests)
			{
				buildOrders.Add(CreateBuildOrder(request));
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
			slotAssigner.AssignSlots(requests, numOfPlatformSlots);
		}

        private IDynamicBuildOrder CreateBuildOrder(IOffensiveRequest request)
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

        public IDynamicBuildOrder CreateAntiAirBuildOrder(int levelNum, ISlotWrapper slotWrapper)
        {
            int numOfDeckSlots = slotWrapper.GetSlotCount(SlotType.Deck);
			int numOfSlotsToUse = Helper.Half(numOfDeckSlots, roundUp: true);

            return
                new AntiUnitBuildOrder(
                    basicDefenceKey: StaticPrefabKeys.Buildings.AntiAirTurret,
                    advancedDefenceKey: StaticPrefabKeys.Buildings.SamSite,
                    buildingKeyHelper: _buildingKeyHelper,
                    numOfSlotsToUse: numOfSlotsToUse);
        }

		public IDynamicBuildOrder CreateAntiNavalBuildOrder(int levelNum, ISlotWrapper slotWrapper)
		{
			int numOfDeckSlots = slotWrapper.GetSlotCount(SlotType.Deck);
			int numOfSlotsToUse = Helper.Half(numOfDeckSlots, roundUp: false);

			return
				new AntiUnitBuildOrder(
                    basicDefenceKey: StaticPrefabKeys.Buildings.AntiShipTurret,
                    advancedDefenceKey: StaticPrefabKeys.Buildings.Mortar,
					buildingKeyHelper: _buildingKeyHelper,
					numOfSlotsToUse: numOfSlotsToUse);
		}
		
        // FELIX
        //public bool IsAntiRocketBuildOrderAvailable(int levelNum)
        //{
        //    return _staticData.IsBuildableAvailable(StaticPrefabKeys.Buildings.TeslaCoil, levelNum);
        //}
    }
}
