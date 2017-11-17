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

        private const float DEFAULT_ZOOM_SPEED = 2;
        public const float MIN_ZOOM_SPEED = 0.1f;
        public const float MAX_ZOOM_SPEED = 3.9f;

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

        public float ZoomSpeed
        {
            get
            {
                return PlayerPrefs.GetFloat(Keys.ZoomSpeed);
            }

            set
            {
                Assert.IsTrue(value >= MIN_ZOOM_SPEED);
                Assert.IsTrue(value <= MAX_ZOOM_SPEED);
                PlayerPrefs.SetFloat(Keys.ZoomSpeed, value);
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
