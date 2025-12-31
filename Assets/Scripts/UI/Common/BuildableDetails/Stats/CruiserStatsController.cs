using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Categorisation;

namespace BattleCruisers.UI.Common.BuildableDetails.Stats
{
    public class CruiserStatsController : StatsController<ICruiser>
    {
        public StarsStatValue health;
        public NumberStatValue platformSlots, deckSlots, utilitySlots, mastSlots;

        public override void Initialise()
        {
            Helper.AssertIsNotNull(health, platformSlots, deckSlots, utilitySlots, mastSlots);

            health.Initialise();
            platformSlots.Initialise();
            deckSlots.Initialise();
            utilitySlots.Initialise();
            mastSlots.Initialise();
        }

        protected override void InternalShowStats(ICruiser item, ICruiser itemToCompareTo)
        {
            health.ShowResult(ValueToStarsConverter.ConvertValueToStars(item.MaxHealth, ValueType.CruiserHealth), HigherIsBetterComparer.CompareStats(item.MaxHealth, itemToCompareTo.MaxHealth));

            int platformSlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Platform);
            platformSlots.ShowResult(platformSlotCount, HigherIsBetterComparer.CompareStats(platformSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Platform)));

            int deckSlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Deck);
            deckSlots.ShowResult(deckSlotCount, HigherIsBetterComparer.CompareStats(deckSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Deck)));

            int utilitySlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Utility);
            utilitySlots.ShowResult(utilitySlotCount, HigherIsBetterComparer.CompareStats(utilitySlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Utility)));

            int mastSlotCount = item.SlotNumProvider.GetSlotCount(SlotType.Mast);
            mastSlots.ShowResult(mastSlotCount, HigherIsBetterComparer.CompareStats(mastSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(SlotType.Mast)));
        }

        protected override void InternalShowStatsOfVariant(ICruiser item, VariantPrefab variant, ICruiser itemToCompareTo)
        {
        }
    }
}