using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Fetchers
{
    public interface ISpriteProvider
    {
        ISpriteWrapper GetSlotSprite(SlotType slotType);
    }
}
