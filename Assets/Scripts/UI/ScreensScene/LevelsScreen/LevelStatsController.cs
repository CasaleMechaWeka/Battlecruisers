using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Common.BuildableDetails.Stats;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class LevelStatsController : MonoBehaviour
    {
        private const int EXPECTED_NUM_OF_STAR = 5;

        public void Initialise(Difficulty? levelCompletedDifficulty)
        {
            StarController[] stars = GetComponentsInChildren<StarController>();
            Assert.AreEqual(EXPECTED_NUM_OF_STAR, stars.Length);

            int numOfStarsToShow = FindNumOfStarsToShow(levelCompletedDifficulty);

            for (int i = 0; i < stars.Length; ++i)
            {
                StarController star = stars[i];
                star.Initialise();
                star.Enabled = numOfStarsToShow > i;
            }
        }

        private int FindNumOfStarsToShow(Difficulty? levelCompletedDifficulty)
        {
            if (levelCompletedDifficulty == null)
            {
                return 0;
            }

            Difficulty completedDifficulty = (Difficulty)levelCompletedDifficulty;

            switch (completedDifficulty)
            {
                case Difficulty.Easy:
                    return 1;
                case Difficulty.Normal:
                    return 2;
                case Difficulty.Hard:
                    return 3;
                case Difficulty.Harder:
                    return 4;
                case Difficulty.Insane:
                    return 5;
                default:
                    throw new ArgumentException();
            }
        }
    }
}