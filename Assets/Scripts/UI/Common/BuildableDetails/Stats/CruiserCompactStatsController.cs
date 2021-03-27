using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class CruiserCompactStatsController : StatsController<ICruiser>
	{
        public StatsCompactStarsController health;
        public StatsRowNumberController platformSlots, deckSlots, utilitySlots, mastSlots;

        public override void Initialise()
        {
            base.Initialise();

            Helper.AssertIsNotNull(health, platformSlots, deckSlots, utilitySlots, mastSlots);

            health.Initialise();
            platformSlots.Initialise();
            deckSlots.Initialise();
            utilitySlots.Initialise();
            mastSlots.Initialise();
        }

		protected override void InternalShowStats(ICruiser item, ICruiser itemToCompareTo)
		{
            health.ShowResult(_cruiserHealthConverter.ConvertValueToStars(item.MaxHealth), _higherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

			int platformSlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Platform);
			platformSlots.ShowResult(platformSlotCount, _higherIsBetterComparer.CompareStats(platformSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Platform)));

			int deckSlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Deck);
			deckSlots.ShowResult(deckSlotCount, _higherIsBetterComparer.CompareStats(deckSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Deck)));

			int utilitySlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Utility);
			utilitySlots.ShowResult(utilitySlotCount, _higherIsBetterComparer.CompareStats(utilitySlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Utility)));

			int mastSlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Mast);
			mastSlots.ShowResult(mastSlotCount, _higherIsBetterComparer.CompareStats(mastSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Mast)));
		}
	}
}