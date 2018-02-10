using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public class CruiserStatsController : StatsController<ICruiser>
	{
		public StatsRowNumberController healthRow, droneRow, platformSlotsRow, deckSlotsRow, utilitySlotsRow, mastSlotsRow;

		protected override void InternalShowStats(ICruiser item, ICruiser itemToCompareTo)
		{
			healthRow.Initialise(item.MaxHealth, _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));
            // FELIX  Remove.  All cruisers should start with 4 drones.  Raptor has extra, but that should be in description!
            // Makes more sense, as other bonuses will be described in description, not have their own stats row.
			droneRow.Initialise(item.NumOfDrones, _higherIsBetterComparer.CompareStats(item.NumOfDrones, itemToCompareTo.NumOfDrones));

			int platformSlotCount = item.SlotWrapper.GetSlotCount(SlotType.Platform);
			platformSlotsRow.Initialise(platformSlotCount, _higherIsBetterComparer.CompareStats(platformSlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Platform)));

			int deckSlotCount = item.SlotWrapper.GetSlotCount(SlotType.Deck);
			deckSlotsRow.Initialise(deckSlotCount, _higherIsBetterComparer.CompareStats(deckSlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Deck)));

			int utilitySlotCount = item.SlotWrapper.GetSlotCount(SlotType.Utility);
			utilitySlotsRow.Initialise(utilitySlotCount, _higherIsBetterComparer.CompareStats(utilitySlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Utility)));

			int mastSlotCount = item.SlotWrapper.GetSlotCount(SlotType.Mast);
			mastSlotsRow.Initialise(mastSlotCount, _higherIsBetterComparer.CompareStats(mastSlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Mast)));
		}
	}
}