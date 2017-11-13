using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine.Assertions;

namespace BattleCruisers.Fetchers
{
    public class SpriteProvider : ISpriteProvider
    {
        private readonly ISpriteFetcher _spriteFetcher;

        private const string SLOT_SPRITES_BASE_PATH = "Sprites/Slots/";
        private const string SLOT_SPRITE_NAME_PREFIX = "slot-";

        private const string AIRCRAFT_SPRITES_BASE_PATH = "Sprites/Buildables/Units/Aircraft/";
        private const string BOMBER_SPRITE_NAME = "bomber";
        private const int NUM_OF_BOMBER_SPRITES = 8;

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

        /// <returns>
        /// A list of bomber sprites, with the first sprite being the least turned
        /// (side on view, no wings showing) and the last sprite being the most
        /// turned (top view, both wings fully showing).
        /// </returns>
        public IList<ISpriteWrapper> GetBomberSprites()
        {
            string spritePath = GetBomberFilePath();
            IList<ISpriteWrapper> bomberSprites = _spriteFetcher.GetMultiSprites(spritePath);
            Assert.AreEqual(NUM_OF_BOMBER_SPRITES, bomberSprites.Count);

            // Reverse order, because the sprites are provided in
            // most turned to least turned, whereas we want to return least
            // turend to most turned.
            return 
                bomberSprites
                    .Reverse()
                    .ToList();
        }

        private string GetBomberFilePath()
        {
            return AIRCRAFT_SPRITES_BASE_PATH + BOMBER_SPRITE_NAME;
        }
    }
}
