using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPFreeSlotFilter : IFilter<PvPSlot>
    {
        public bool IsMatch(PvPSlot slot)
        {
            return slot.IsFree;
        }
        public bool IsMatch(PvPSlot slot, VariantPrefab variant)
        {
            return slot.IsFree;
        }
    }
}
