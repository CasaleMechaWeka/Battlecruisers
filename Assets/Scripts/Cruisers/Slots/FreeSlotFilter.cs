using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Slots
{
    public class FreeSlotFilter : IFilter<Slot>
    {
        public bool IsMatch(Slot slot)
        {
            return slot.IsFree;
        }
        public bool IsMatch(Slot slot, VariantPrefab variant)
        {
            return slot.IsFree;
        }
    }
}
