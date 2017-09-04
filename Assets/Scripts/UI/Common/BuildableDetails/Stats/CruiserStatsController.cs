using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public class CruiserStatsController : StatsController<Cruiser>
	{
		public StatsRowNumberController healthRow, droneRow, platformSlotsRow, deckSlotsRow, utilitySlotsRow, mastSlotsRow;

		private const string PLATFORM_SLOTS = "Platform Slots";
		private const string DECK_SLOTS = "Deck Slots";
		private const string UTILITY_SLOTS = "Utility Slots";
		private const string MAST_SLOTS = "Mast Slots";

		protected override void InternalShowStats(Cruiser item, Cruiser itemToCompareTo)
		{
			healthRow.Initialise(HEALTH_LABEL, item.Health, _higherIsBetterComparer.CompareStats(item.Health, itemToCompareTo.Health));
			droneRow.Initialise(DRONES_LABEL, item.numOfDrones, _higherIsBetterComparer.CompareStats(item.numOfDrones, itemToCompareTo.numOfDrones));

			int platformSlotCount = item.SlotWrapper.GetSlotCount(SlotType.Platform);
			platformSlotsRow.Initialise(PLATFORM_SLOTS, platformSlotCount, _higherIsBetterComparer.CompareStats(platformSlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Platform)));

			int deckSlotCount = item.SlotWrapper.GetSlotCount(SlotType.Deck);
			deckSlotsRow.Initialise(DECK_SLOTS, deckSlotCount, _higherIsBetterComparer.CompareStats(deckSlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Deck)));

			int utilitySlotCount = item.SlotWrapper.GetSlotCount(SlotType.Utility);
			utilitySlotsRow.Initialise(UTILITY_SLOTS, utilitySlotCount, _higherIsBetterComparer.CompareStats(utilitySlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Utility)));

			int mastSlotCount = item.SlotWrapper.GetSlotCount(SlotType.Mast);
			mastSlotsRow.Initialise(MAST_SLOTS, mastSlotCount, _higherIsBetterComparer.CompareStats(mastSlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Mast)));
		}
	}
}