using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers
{
    public class SpriteProvider : ISpriteProvider
    {
        private readonly ISpriteFetcher _spriteFetcher;

        private const string SLOT_SPRITES_BASE_PATH = "Sprites/Slots/";
        private const string SLOT_SPRITE_NAME_PREFIX = "slot-";

        private const string AIRCRAFT_SPRITES_BASE_PATH = "Sprites/Buildables/Units/Aircraft/";
        private const string BOMBER_SPRITE_NAME = "bomber";
        private const int NUM_OF_BOMBER_SPRITES = 8;
        private const string FIGHTER_SPRITE_NAME = "fighter";
        private const int NUM_OF_FIGHTER_SPRITES = 7;

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

        public IList<ISpriteWrapper> GetBomberSprites()
        {
            string spritePath = GetBomberFilePath();
            return GetAircraftSprites(spritePath, NUM_OF_BOMBER_SPRITES);
        }

        private string GetBomberFilePath()
        {
            return AIRCRAFT_SPRITES_BASE_PATH + BOMBER_SPRITE_NAME;
        }

        public IList<ISpriteWrapper> GetFighterSprites()
        {
            string spritePath = GetFighterFilePath();
            return GetAircraftSprites(spritePath, NUM_OF_FIGHTER_SPRITES);
        }

        private string GetFighterFilePath()
        {
            return AIRCRAFT_SPRITES_BASE_PATH + FIGHTER_SPRITE_NAME;
        }

        /// <returns>
        /// A list of aircraft sprites, with the first sprite being the least turned
        /// (side on view, no wings showing) and the last sprite being the most
        /// turned (top view, both wings fully showing).
        /// </returns>
        public IList<ISpriteWrapper> GetAircraftSprites(string spritePath, int expectedNumOfSprites)
        {
            IList<ISpriteWrapper> aircraftSprites = _spriteFetcher.GetMultiSprites(spritePath);
            Assert.AreEqual(expectedNumOfSprites, aircraftSprites.Count);

            // Reverse order, because the sprites are provided in most turned to 
            // least turned, whereas we want to return least turned to most turned.
            return
                aircraftSprites
                    .Reverse()
                    .ToList();
        }
    }
}
