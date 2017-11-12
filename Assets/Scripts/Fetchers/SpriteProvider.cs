using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine.Assertions;

namespace BattleCruisers.Fetchers
{
    // FELIX  Test :)
    public class SpriteProvider : ISpriteProvider
    {
        private readonly ISpriteFetcher _spriteFetcher;

        private const string SLOT_SPRITES_BASE_PATH = "Sprites/Slots/";
        private const string SLOT_SPRITE_NAME_PREFIX = "slot-";

        public SpriteProvider(ISpriteFetcher spriteFetcher)
        {
            Assert.IsNotNull(spriteFetcher);
            _spriteFetcher = spriteFetcher;
        }

        public ISpriteWrapper GetSlotSprite(SlotType slotType)
        {
            string spritePath = GetSlotFilePath(slotType);
            return _spriteFetcher.GetSprite(spritePath);
        }

        private string GetSlotFilePath(SlotType slotType)
        {
            return SLOT_SPRITES_BASE_PATH + SLOT_SPRITE_NAME_PREFIX + slotType.ToString().ToLower();
        }
    }
}
