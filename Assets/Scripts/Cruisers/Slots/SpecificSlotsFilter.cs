using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Slots
{
    public class SpecificSlotsFilter : IFilter<ISlot>
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
