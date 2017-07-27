using BattleCruisers.Cruisers;

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
			ICruiserStats stats = item.Stats;
			ICruiserStats otherStats = itemToCompareTo.Stats;

			healthRow.Initialise(HEALTH_LABEL, stats.Health, _higherIsBetterComparer.CompareStats(stats.Health, otherStats.Health));
			droneRow.Initialise(DRONES_LABEL, stats.NumOfDrones, _higherIsBetterComparer.CompareStats(stats.NumOfDrones, otherStats.NumOfDrones));

            platformSlotsRow.Initialise(PLATFORM_SLOTS, stats.NumOfPlatformSlots, _higherIsBetterComparer.CompareStats(stats.NumOfPlatformSlots, otherStats.NumOfPlatformSlots));
            deckSlotsRow.Initialise(DECK_SLOTS, stats.NumOfDeckSlots, _higherIsBetterComparer.CompareStats(stats.NumOfDeckSlots, otherStats.NumOfDeckSlots));
            utilitySlotsRow.Initialise(UTILITY_SLOTS, stats.NumOfUtilitySlots, _higherIsBetterComparer.CompareStats(stats.NumOfUtilitySlots, otherStats.NumOfUtilitySlots));
            mastSlotsRow.Initialise(MAST_SLOTS, stats.NumOfMastSlots, _higherIsBetterComparer.CompareStats(stats.NumOfMastSlots, otherStats.NumOfMastSlots));
		}
	}
}