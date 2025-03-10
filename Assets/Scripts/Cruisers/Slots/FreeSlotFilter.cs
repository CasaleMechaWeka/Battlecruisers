using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Cruisers.Slots
{
    public class FreeSlotFilter : ISlotFilter
    {
        public bool IsMatch(ISlot slot)
        {
            return slot.IsFree;
        }
        public bool IsMatch(ISlot slot, VariantPrefab variant)
        {
            return slot.IsFree;
        }
    }
}
