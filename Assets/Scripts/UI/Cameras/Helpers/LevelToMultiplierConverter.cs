using BattleCruisers.Data.Models;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class LevelToMultiplierConverter
    {
        public float LevelToMultiplier(int settingLevel)
        {
            Assert.IsTrue(settingLevel >= SettingsModel.MIN_SCROLL_SPEED_LEVEL);
            Assert.IsTrue(settingLevel <= SettingsModel.MAX_SCROLL_SPEED_LEVEL);

            return settingLevel switch
            {
                1 => 0.2f,
                2 => 0.4f,
                3 => 0.6f,
                4 => 0.8f,
                // Default settings
                5 => 1,
                6 => 1.5f,
                7 => 2,
                8 => 4,
                9 => 8,
                _ => throw new ArgumentException(),
            };
        }
    }
}