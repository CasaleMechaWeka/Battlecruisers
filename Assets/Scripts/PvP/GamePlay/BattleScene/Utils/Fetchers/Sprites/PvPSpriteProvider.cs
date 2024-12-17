using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites
{
    public class PvPSpriteProvider : IPvPSpriteProvider
    {
        private readonly IPvPSpriteFetcher _spriteFetcher;
        private const string UNIT_SPRITES_PATH = "Assets/Resources_moved/Sprites/Buildables/Units/Aircraft/";
        private const string SPRITES_FILE_EXTENSION = ".png";

        public PvPSpriteProvider(IPvPSpriteFetcher spriteFetcher)
        {
            Assert.IsNotNull(spriteFetcher);
            _spriteFetcher = spriteFetcher;
        }

        /// <returns>
        /// A list of aircraft sprites, with the first sprite being the least turned
        /// (side on view, no wings showing) and the last sprite being the most
        /// turned (top view, both wings fully showing).
        /// </returns>
        public async Task<IList<IPvPSpriteWrapper>> GetAircraftSpritesAsync(PrefabKeyName prefabKeyName)
        {
            (string, int) spriteData = prefabKeyName switch
            {
                PrefabKeyName.Unit_Bomber => ("bomber", 8),
                PrefabKeyName.Unit_Fighter => ("fighter", 7),
                PrefabKeyName.Unit_Gunship => ("gunship", 7),
                PrefabKeyName.Unit_SteamCopter => ("SteamCopter", 7),
                PrefabKeyName.Unit_StratBomber => ("StratBomber", 12),
                PrefabKeyName.Unit_SpyPlane => ("SpyPlane", 12),
                PrefabKeyName.Unit_MissileFighter => ("MissileFighter", 9),
                _ => throw new ArgumentException("PrefabKeyName '" + prefabKeyName + "' is not an Aircraft!")
            };

            IList<IPvPSpriteWrapper> aircraftSprites = await _spriteFetcher.GetMultiSpritesAsync(
                UNIT_SPRITES_PATH + spriteData.Item1 + SPRITES_FILE_EXTENSION);
            Assert.AreEqual(spriteData.Item2, aircraftSprites.Count);

            // Reverse order, because the sprites are provided in most turned to 
            // least turned, whereas we want to return least turned to most turned.
            return aircraftSprites.Reverse().ToList();
        }
    }
}
