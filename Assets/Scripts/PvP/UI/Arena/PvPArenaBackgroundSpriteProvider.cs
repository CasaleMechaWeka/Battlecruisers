using System;
using UnityEngine.Assertions;
using System.Threading.Tasks;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;


namespace BattleCruisers.Utils.Fetchers.Sprites
{
    public class PvPArenaBackgroundSpriteProvider : IPvPArenaBackgroundSpriteProvider
    {
        private readonly ISpriteFetcher _spriteFetcher;
        private const string SPRITES_PATH = "Assets/Resources_moved/Sprites/Skies/Backgrounds/";
        private const string PRACTICEWRECKYARDS_SPRITE = "PvPBackgroundWreckyards.png";
        private const string OZPENITENTIARY_SPRITE = "PvPBackgroundOz.png";
        private const string SANFRANCISCOFIGHTCLUB_SPRITE = "PvPBackgroundFightClub.png";
        private const string UACBATTLENIGHT_SPRITE = "PvPBackgroundUACBattleNight.png";
        private const string NUCLEARDOME_SPRITE = "PvPBackgroundNuclearDome.png";
        private const string UACARENA_SPRITE = "PvPBackgroundUACArena.png";
        private const string RIOBATTLESPORT_SPRITE = "PvPBackgroundRio.png";
        private const string UACULTIMATE_SPRITE = "PvPBackgroundUACUltimate.png";
        private const string MERCENARY_SPRITE = "PvPBackgroundMercenaryOne.png";


        public PvPArenaBackgroundSpriteProvider(ISpriteFetcher spriteFetcher)
        {
            Assert.IsNotNull(spriteFetcher);
            _spriteFetcher = spriteFetcher;
        }


        public async Task<ISpriteWrapper> GetSpriteAsync(Map map)
        {
            string spritePath = GetSpritePath(map);
            return await _spriteFetcher.GetSpriteAsync(spritePath);
        }


        private string GetSpritePath(Map map)
        {
            switch (map)
            {
                case Map.PracticeWreckyards:
                    return SPRITES_PATH + PRACTICEWRECKYARDS_SPRITE;
                case Map.OzPenitentiary:
                    return SPRITES_PATH + OZPENITENTIARY_SPRITE;
                case Map.SanFranciscoFightClub:
                    return SPRITES_PATH + SANFRANCISCOFIGHTCLUB_SPRITE;
                case Map.UACBattleNight:
                    return SPRITES_PATH + UACBATTLENIGHT_SPRITE;
                case Map.NuclearDome:
                    return SPRITES_PATH + NUCLEARDOME_SPRITE;
                case Map.UACArena:
                    return SPRITES_PATH + UACARENA_SPRITE;
                case Map.RioBattlesport:
                    return SPRITES_PATH + RIOBATTLESPORT_SPRITE;
                case Map.UACUltimate:
                    return SPRITES_PATH + UACULTIMATE_SPRITE;
                case Map.MercenaryOne:
                    return SPRITES_PATH + MERCENARY_SPRITE;
                default:
                    throw new ArgumentException($"Unknown map {map}");
            }
        }
    }
}
