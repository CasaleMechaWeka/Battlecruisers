using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Fetchers.Sprites
{
    public class DifficultySpritesProvider : IDifficultySpritesProvider
    {
        private readonly ISpriteFetcher _spriteFetcher;

        private const string SPRITES_PATH = "Assets/Resources_moved/Sprites/UI/ScreensScene/LevelsScreen/";
        private const string EASY_SPRITE = "MedalStars-1.png";
        private const string NORMAL_SPRITE = "MedalStars-1.png";
        private const string HARD_SPRITE = "MedalStars-2.png";
        private const string HARDER_SPRITE = "MedalStars-3.png";

        public DifficultySpritesProvider(ISpriteFetcher spriteFetcher)
        {
            Assert.IsNotNull(spriteFetcher);
            _spriteFetcher = spriteFetcher;
        }

        public async Task<ISpriteWrapper> GetSpriteAsync(Difficulty difficulty)
        {
            string spritePath = GetSpritePath(difficulty);
            return await _spriteFetcher.GetSpriteAsync(spritePath);
        }

        private string GetSpritePath(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return SPRITES_PATH + EASY_SPRITE;
                case Difficulty.Normal:
                    return SPRITES_PATH + NORMAL_SPRITE;
                case Difficulty.Hard:
                    return SPRITES_PATH + HARD_SPRITE;
                case Difficulty.Harder:
                    return SPRITES_PATH + HARDER_SPRITE;
                default:
                    throw new ArgumentException($"Unknown difficulty {difficulty}");
            }
        }
    }
}