using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private static class Keys
        {
            public const string Difficulty = "Difficulty";
            public const string ZoomSpeed = "ZoomSpeed";
        }

        private const int DEFAULT_ZOOM_SPEED = 5;
        public const int MIN_ZOOM_SPEED = 1;
        public const int MAX_ZOOM_SPEED = 9;

        public Difficulty AIDifficulty
        {
            get
            {
                return (Difficulty)Enum.Parse(typeof(Difficulty), PlayerPrefs.GetString(Keys.Difficulty));
            }
            set
            {
                PlayerPrefs.SetString(Keys.Difficulty, value.ToString());
            }
        }

        public int ZoomSpeed
        {
            get
            {
                return PlayerPrefs.GetInt(Keys.ZoomSpeed);
            }

            set
            {
                Assert.IsTrue(value >= MIN_ZOOM_SPEED);
                Assert.IsTrue(value <= MAX_ZOOM_SPEED);
                PlayerPrefs.SetInt(Keys.ZoomSpeed, value);
            }
        }

        public SettingsManager()
        {
            if (!PlayerPrefs.HasKey(Keys.Difficulty))
            {
                CreateSettings();
            }
        }

        private void CreateSettings()
        {
            AIDifficulty = Difficulty.Normal;
            ZoomSpeed = DEFAULT_ZOOM_SPEED;

            Save();
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}
