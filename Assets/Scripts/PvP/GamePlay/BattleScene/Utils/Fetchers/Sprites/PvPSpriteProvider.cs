using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites
{
    public class PvPSpriteProvider : IPvPSpriteProvider
    {
        private readonly IPvPSpriteFetcher _spriteFetcher;

        private const string BOMBER_SPRITE_NAME = "bomber";
        private const int NUM_OF_BOMBER_SPRITES = 8;
        private const string FIGHTER_SPRITE_NAME = "fighter";
        private const int NUM_OF_FIGHTER_SPRITES = 7;
        private const string GUNSHIP_SPRITE_NAME = "gunship";
        private const int NUM_OF_GUNSHIP_SPRITES = 7;
        private const string STEAMCOPTER_SPRITE_NAME = "SteamCopter";
        private const int NUM_OF_STEAMCOPTER_SPRITES = 7;
        private const string UNIT_SPRITES_PATH = "Assets/Resources_moved/Sprites/Buildables/Units/Aircraft/";
        private const string SPRITES_FILE_EXTENSION = ".png";

        public PvPSpriteProvider(IPvPSpriteFetcher spriteFetcher)
        {
            Assert.IsNotNull(spriteFetcher);
            _spriteFetcher = spriteFetcher;
        }

        public async Task<IList<IPvPSpriteWrapper>> GetBomberSpritesAsync()
        {
            return await GetAircraftSpritesAsync(GetSpritePath(BOMBER_SPRITE_NAME), NUM_OF_BOMBER_SPRITES);
        }

        public async Task<IList<IPvPSpriteWrapper>> GetFighterSpritesAsync()
        {
            return await GetAircraftSpritesAsync(GetSpritePath(FIGHTER_SPRITE_NAME), NUM_OF_FIGHTER_SPRITES);
        }

        public async Task<IList<IPvPSpriteWrapper>> GetGunshipSpritesAsync()
        {
            return await GetAircraftSpritesAsync(GetSpritePath(GUNSHIP_SPRITE_NAME), NUM_OF_GUNSHIP_SPRITES);
        }

        public async Task<IList<IPvPSpriteWrapper>> GetSteamCopterSpritesAsync()
        {
            return await GetAircraftSpritesAsync(GetSpritePath(STEAMCOPTER_SPRITE_NAME), NUM_OF_STEAMCOPTER_SPRITES);
        }

        private string GetSpritePath(string spriteName)
        {
            return UNIT_SPRITES_PATH + spriteName + SPRITES_FILE_EXTENSION;
        }

        /// <returns>
        /// A list of aircraft sprites, with the first sprite being the least turned
        /// (side on view, no wings showing) and the last sprite being the most
        /// turned (top view, both wings fully showing).
        /// </returns>
        private async Task<IList<IPvPSpriteWrapper>> GetAircraftSpritesAsync(string spritePath, int expectedNumOfSprites)
        {
            IList<IPvPSpriteWrapper> aircraftSprites = await _spriteFetcher.GetMultiSpritesAsync(spritePath);
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
