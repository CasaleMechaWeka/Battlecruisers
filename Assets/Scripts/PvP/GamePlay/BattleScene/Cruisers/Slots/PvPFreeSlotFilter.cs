using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPFreeSlotFilter : IFilter<IPvPSlot>
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
