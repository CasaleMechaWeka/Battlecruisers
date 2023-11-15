using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats
{
    public class PvPCruiserStatsController : PvPStatsController<IPvPCruiser>
    {
        public PvPStarsStatValue health;
        public PvPNumberStatValue platformSlots, deckSlots, utilitySlots, mastSlots;

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

            int platformSlotCount = item.SlotNumProvider.GetSlotCount(PvPSlotType.Platform);
            platformSlots.ShowResult(platformSlotCount, _higherIsBetterComparer.CompareStats(platformSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(PvPSlotType.Platform)));

            int deckSlotCount = item.SlotNumProvider.GetSlotCount(PvPSlotType.Deck);
            deckSlots.ShowResult(deckSlotCount, _higherIsBetterComparer.CompareStats(deckSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(PvPSlotType.Deck)));

            int utilitySlotCount = item.SlotNumProvider.GetSlotCount(PvPSlotType.Utility);
            utilitySlots.ShowResult(utilitySlotCount, _higherIsBetterComparer.CompareStats(utilitySlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(PvPSlotType.Utility)));

            int mastSlotCount = item.SlotNumProvider.GetSlotCount(PvPSlotType.Mast);
            mastSlots.ShowResult(mastSlotCount, _higherIsBetterComparer.CompareStats(mastSlotCount, itemToCompareTo.SlotNumProvider.GetSlotCount(PvPSlotType.Mast)));
        }
    }
}