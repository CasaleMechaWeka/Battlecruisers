using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails.Stats;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
	// FELIX  Generic way of applying stats?
	public class CruiserStatsController : StatsController<Cruiser>
	{
		public StatsRowNumberController healthRow, droneRow, platformSlotsRow, deckSlotsRow, utilitySlotsRow, mastSlotsRow;

		private const string PLATFORM_SLOTS = "Platform Slots";
		private const string DECK_SLOTS = "Deck Slots";
		private const string UTILITY_SLOTS = "Utility Slots";
		private const string MAST_SLOTS = "Mast Slots";

		protected override void InternalShowStats(Cruiser item, Cruiser itemToCompareTo)
		{
			healthRow.Iniitalise(HEALTH_LABEL, item.Health, _higherIsBetterComparer.CompareStats(item.Health, itemToCompareTo.Health));
			droneRow.Initialise(DRONES_LABEL, item.numOfDrones, _higherIsBetterComparer.CompareStats(item.numOfDrones, itemToCompareTo.numOfDrones));

			int platformSlotCount = item.GetSlotCount(SlotType.Platform);
			platformSlotsRow.Iniitalise(PLATFORM_SLOTS, platformSlotCount, _higherIsBetterComparer.CompareStats(platformSlotCount, itemToCompareTo.GetSlotCount(SlotType.Platform)));

			int deckSlotCount = item.GetSlotCount(SlotType.Deck);
			deckSlotsRow.Iniitalise(DECK_SLOTS, deckSlotCount, _higherIsBetterComparer.CompareStats(deckSlotCount, itemToCompareTo.GetSlotCount(SlotType.Deck)));

			int utilitySlotCount = item.GetSlotCount(SlotType.Utility);
			utilitySlotsRow.Iniitalise(UTILITY_SLOTS, utilitySlotCount, _higherIsBetterComparer.CompareStats(utilitySlotCount, itemToCompareTo.GetSlotCount(SlotType.Utility)));

			int mastSlotCount = item.GetSlotCount(SlotType.Mast);
			mastSlotsRow.Iniitalise(MAST_SLOTS, mastSlotCount, _higherIsBetterComparer.CompareStats(mastSlotCount, itemToCompareTo.GetSlotCount(SlotType.Mast)));
		}
	}
}