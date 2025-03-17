using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public class PvPCruiserStatsController : StatsController<IPvPCruiser>
    {
        public StarsStatValue health;
        public NumberStatValue platformSlots, deckSlots, utilitySlots, mastSlots;

        public override void Initialise()
        {
            base.Initialise();

            PvPHelper.AssertIsNotNull(health, platformSlots, deckSlots, utilitySlots, mastSlots);

            health.Initialise();
            platformSlots.Initialise();
            deckSlots.Initialise();
            utilitySlots.Initialise();
            mastSlots.Initialise();
        }

        protected override void InternalShowStats(IPvPCruiser item, IPvPCruiser itemToCompareTo)
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

        protected override void InternalShowStatsOfVariant(IPvPCruiser item, VariantPrefab variant, IPvPCruiser itemToCompareTo)
        {
        }
    }
}