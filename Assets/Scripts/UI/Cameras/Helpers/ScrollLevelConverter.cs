using BattleCruisers.Data.Models;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class ScrollLevelConverter : ILevelToMultiplierConverter
    {
        public float LevelToMultiplier(int scrollLevel)
        {
            Assert.IsTrue(scrollLevel >= SettingsModel.MIN_SCROLL_SPEED_LEVEL);
            Assert.IsTrue(scrollLevel <= SettingsModel.MAX_SCROLL_SPEED_LEVEL);

            switch (scrollLevel)
            {
                case 1:
                    return 0.2f;

                case 2:
                    return 0.4f;

                case 3:
                    return 0.6f;

                case 4:
                    return 0.8f;

                // Default settings
                case 5:
                    return 1;

                case 6:
                    return 1.5f;

                case 7:
                    return 2;

                case 8:
                    return 4;

                case 9:
                    return 8;

                default:
                    throw new ArgumentException();
            }
        }
    }
}