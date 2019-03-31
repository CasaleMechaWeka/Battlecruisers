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
			public const string ZoomSpeedLevel = "ZoomSpeed";
            public const string ScrollSpeed = "ScrollSpeed";
        }

        private const int DEFAULT_ZOOM_SPEED_LEVEL = 5;
        public const int MIN_ZOOM_SPEED_LEVEL = 1;
        public const int MAX_ZOOM_SPEED = 9;

		private const float DEFAULT_SCROLL_SPEED = 2;
		public const float MIN_SCROLL_SPEED = 0.1f;
		public const float MAX_SCROLL_SPEED = 3.9f;

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

        public int ZoomSpeedLevel
        {
            get
            {
                return PlayerPrefs.GetInt(Keys.ZoomSpeedLevel);
            }
            set
            {
                Assert.IsTrue(value >= MIN_ZOOM_SPEED_LEVEL);
                Assert.IsTrue(value <= MAX_ZOOM_SPEED);
                PlayerPrefs.SetInt(Keys.ZoomSpeedLevel, value);
            }
        }

        public float ScrollSpeed
		{
			get
			{
				return PlayerPrefs.GetFloat(Keys.ScrollSpeed);
			}
			set
			{
				Assert.IsTrue(value >= MIN_SCROLL_SPEED);
				Assert.IsTrue(value <= MAX_SCROLL_SPEED);
				PlayerPrefs.SetFloat(Keys.ScrollSpeed, value);
			}
		}

        public SettingsManager()
        {
            if (!PlayerPrefs.HasKey(Keys.Difficulty)
                || !PlayerPrefs.HasKey(Keys.ZoomSpeedLevel)
                || !PlayerPrefs.HasKey(Keys.ScrollSpeed))
            {
                CreateSettings();
            }
        }

        private void CreateSettings()
        {
            AIDifficulty = Difficulty.Normal;
            ZoomSpeedLevel = DEFAULT_ZOOM_SPEED_LEVEL;
			ScrollSpeed = DEFAULT_SCROLL_SPEED;

            Save();
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}
