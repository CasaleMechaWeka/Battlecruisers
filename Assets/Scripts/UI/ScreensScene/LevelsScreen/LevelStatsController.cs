using BattleCruisers.Data.Settings;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelStatsController : MonoBehaviour
    {
        public Image difficultyCompletedImage;

        public void Initialise(Difficulty? levelCompletedDifficulty, Sprite[] difficultyIndicators)
        {
            Assert.IsNotNull(difficultyIndicators);
            Assert.IsNotNull(difficultyCompletedImage);

            if (levelCompletedDifficulty == null)
            {
                difficultyCompletedImage.enabled = false;
                return;
            }

            int difficultyValue = (int)levelCompletedDifficulty - 1;
            if (difficultyValue < 0)
                difficultyValue = 0;
            difficultyCompletedImage.sprite = difficultyIndicators[difficultyValue];
        }

        public void SetColour(Color color)
        {
            difficultyCompletedImage.color = color;
        }
    }
}