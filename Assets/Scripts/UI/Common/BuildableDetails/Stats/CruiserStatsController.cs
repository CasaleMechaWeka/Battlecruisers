using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class CruiserStatsController : StatsController<ICruiser>
	{
        private StatsRowStarsController _healthRow;
        private StatsRowNumberController _platformSlotsRow, _deckSlotsRow, _utilitySlotsRow, _mastSlotsRow;

        public override void Initialise()
        {
            base.Initialise();

            _healthRow = transform.FindNamedComponent<StatsRowStarsController>("HealthRow");
            _healthRow.Initialise();

            _platformSlotsRow = transform.FindNamedComponent<StatsRowNumberController>("PlatformSlotsRow");
            _platformSlotsRow.Initialise();

            _deckSlotsRow = transform.FindNamedComponent<StatsRowNumberController>("DeckSlotsRow");
            _deckSlotsRow.Initialise();

            _utilitySlotsRow = transform.FindNamedComponent<StatsRowNumberController>("UtilitySlotsRow");
            _utilitySlotsRow.Initialise();

            _mastSlotsRow = transform.FindNamedComponent<StatsRowNumberController>("MastSlotsRow");
            _mastSlotsRow.Initialise();
        }

		protected override void InternalShowStats(ICruiser item, ICruiser itemToCompareTo)
		{
            _healthRow.ShowResult(_cruiserHealthConverter.ConvertValueToStars(item.MaxHealth), _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

			int platformSlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Platform);
			_platformSlotsRow.ShowResult(platformSlotCount, _higherIsBetterComparer.CompareStats(platformSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Platform)));

			int deckSlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Deck);
			_deckSlotsRow.ShowResult(deckSlotCount, _higherIsBetterComparer.CompareStats(deckSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Deck)));

			int utilitySlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Utility);
			_utilitySlotsRow.ShowResult(utilitySlotCount, _higherIsBetterComparer.CompareStats(utilitySlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Utility)));

			int mastSlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Mast);
			_mastSlotsRow.ShowResult(mastSlotCount, _higherIsBetterComparer.CompareStats(mastSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Mast)));
		}
	}
}