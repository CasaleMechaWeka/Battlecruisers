using System.Collections.Generic;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Utils.Fetchers
{
    public interface ISpriteProvider
    {
        ISpriteWrapper GetSlotSprite(SlotType slotType);
        IList<ISpriteWrapper> GetBomberSprites();
        IList<ISpriteWrapper> GetFighterSprites();
    }
}
