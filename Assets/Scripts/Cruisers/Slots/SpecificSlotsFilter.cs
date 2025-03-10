using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Cruisers.Slots
{
    public class SpecificSlotsFilter : ISlotFilter, ISlotPermitter
    {
        public ISlot PermittedSlot { private get; set; }

        public bool IsMatch(ISlot slot)
        {
            return PermittedSlot == slot;
        }
        public bool IsMatch(ISlot slot, VariantPrefab variant)
        {
            return PermittedSlot == slot;
        }
    }
}
