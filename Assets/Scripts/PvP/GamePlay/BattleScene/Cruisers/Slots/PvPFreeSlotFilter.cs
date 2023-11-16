using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPFreeSlotFilter : IPvPSlotFilter
    {
        public bool IsMatch(IPvPSlot slot)
        {
            return slot.IsFree;
        }
        public bool IsMatch(IPvPSlot slot, VariantPrefab variant)
        {
            return slot.IsFree;
        }
    }
}
