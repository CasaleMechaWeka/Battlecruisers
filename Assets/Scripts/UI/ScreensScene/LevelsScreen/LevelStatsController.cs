using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelStatsController : MonoBehaviour
    {
        public Image difficultyCompletedImage;

        public async Task InitialiseAsync(Difficulty? levelCompletedDifficulty, IDifficultySpritesProvider difficultySpritesProvider)
        {
            Assert.IsNotNull(difficultySpritesProvider);
            Assert.IsNotNull(difficultyCompletedImage);

            if (levelCompletedDifficulty == null)
            {
                difficultyCompletedImage.enabled = false;
                return;
            }

            ISpriteWrapper difficultySprite = await difficultySpritesProvider.GetSpriteAsync((Difficulty)levelCompletedDifficulty);
            difficultyCompletedImage.sprite = difficultySprite.Sprite;
        }

        public void SetColour(Color color)
        {
            difficultyCompletedImage.color = color;
        }
    }
}