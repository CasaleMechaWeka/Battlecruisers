using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Slots
{
    public class SpecificSlotsFilter : IFilter<Slot>
    {
        public Slot PermittedSlot { private get; set; }

        public bool IsMatch(Slot slot)
        {
            return PermittedSlot == slot;
        }
        public bool IsMatch(Slot slot, VariantPrefab variant)
        {
            return PermittedSlot == slot;
        }
    }
}
