using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers
{
    public class SpriteProvider : ISpriteProvider
    {
        private readonly ISpriteFetcher _spriteFetcher;

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

        public async Task<IList<ISpriteWrapper>> GetBomberSpritesAsync()
        {
            string spritePath = GetBomberFilePath();
            // FELIX
            return await GetAircraftSpritesAsync(BOMBER_SPRITE_NAME, NUM_OF_BOMBER_SPRITES);
            //return await GetAircraftSpritesAsync(spritePath, NUM_OF_BOMBER_SPRITES);
        }

        private string GetBomberFilePath()
        {
            return AIRCRAFT_SPRITES_BASE_PATH + BOMBER_SPRITE_NAME;
        }

        public async Task<IList<ISpriteWrapper>> GetFighterSpritesAsync()
        {
            string spritePath = GetFighterFilePath();
            // FELIX
            return await GetAircraftSpritesAsync(FIGHTER_SPRITE_NAME, NUM_OF_FIGHTER_SPRITES);
            //return await GetAircraftSpritesAsync(spritePath, NUM_OF_FIGHTER_SPRITES);
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
        public async Task<IList<ISpriteWrapper>> GetAircraftSpritesAsync(string spritePath, int expectedNumOfSprites)
        {
            IList<ISpriteWrapper> aircraftSprites = await _spriteFetcher.GetMultiSpritesAsync(spritePath);
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
