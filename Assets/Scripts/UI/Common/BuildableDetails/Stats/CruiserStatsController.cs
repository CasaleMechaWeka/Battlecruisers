using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildingDetails.Stats
{
    public class CruiserStatsController : StatsController<ICruiser>
	{
        private StatsRowNumberController _healthRow, _platformSlotsRow, _deckSlotsRow, _utilitySlotsRow, _mastSlotsRow;

        public override void Initialise()
        {
            base.Initialise();

            _healthRow = transform.FindNamedComponent<StatsRowNumberController>("HealthRow");
            _platformSlotsRow = transform.FindNamedComponent<StatsRowNumberController>("PlatformSlotsRow");
            _deckSlotsRow = transform.FindNamedComponent<StatsRowNumberController>("DeckSlotsRow");
            _utilitySlotsRow = transform.FindNamedComponent<StatsRowNumberController>("UtilitySlotsRow");
            _mastSlotsRow = transform.FindNamedComponent<StatsRowNumberController>("MastSlotsRow");
        }

		protected override void InternalShowStats(ICruiser item, ICruiser itemToCompareTo)
		{
            // FELIX  Health row should be stars, like buildables health :)
			_healthRow.Initialise(item.MaxHealth, _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

			int platformSlotCount = item.SlotWrapper.GetSlotCount(SlotType.Platform);
			_platformSlotsRow.Initialise(platformSlotCount, _higherIsBetterComparer.CompareStats(platformSlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Platform)));

			int deckSlotCount = item.SlotWrapper.GetSlotCount(SlotType.Deck);
			_deckSlotsRow.Initialise(deckSlotCount, _higherIsBetterComparer.CompareStats(deckSlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Deck)));

			int utilitySlotCount = item.SlotWrapper.GetSlotCount(SlotType.Utility);
			_utilitySlotsRow.Initialise(utilitySlotCount, _higherIsBetterComparer.CompareStats(utilitySlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Utility)));

			int mastSlotCount = item.SlotWrapper.GetSlotCount(SlotType.Mast);
			_mastSlotsRow.Initialise(mastSlotCount, _higherIsBetterComparer.CompareStats(mastSlotCount, itemToCompareTo.SlotWrapper.GetSlotCount(SlotType.Mast)));
		}
	}
}