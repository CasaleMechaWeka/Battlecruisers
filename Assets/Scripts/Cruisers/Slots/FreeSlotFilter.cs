using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Slots
{
    public class FreeSlotFilter : IFilter<ISlot>
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
