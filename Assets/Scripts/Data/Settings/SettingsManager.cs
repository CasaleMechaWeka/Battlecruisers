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
            public const string MuteMusic = "MuteMusic";
            public const string ShowInGameHints = "ShowInGameHints";
        }

        private const int DEFAULT_ZOOM_SPEED_LEVEL = 5;
        public const int MIN_ZOOM_SPEED_LEVEL = 1;
        public const int MAX_ZOOM_SPEED_LEVEL = 9;

		public const int DEFAULT_SCROLL_SPEED_LEVEL = 5;
		public const int MIN_SCROLL_SPEED_LEVEL = 1;
		public const int MAX_SCROLL_SPEED_LEVEL = 9;

        private const int FALSE = 0;
        private const int TRUE = 1;

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
                Assert.IsTrue(value <= MAX_ZOOM_SPEED_LEVEL);
                PlayerPrefs.SetInt(Keys.ZoomSpeedLevel, value);
            }
        }

        public int ScrollSpeedLevel
		{
			get
			{
				return PlayerPrefs.GetInt(Keys.ScrollSpeed);
			}
			set
			{
				Assert.IsTrue(value >= MIN_SCROLL_SPEED_LEVEL);
				Assert.IsTrue(value <= MAX_SCROLL_SPEED_LEVEL);
				PlayerPrefs.SetInt(Keys.ScrollSpeed, value);
			}
		}

        public bool MuteMusic
        {
            get
            {
                return PlayerPrefs.GetInt(Keys.MuteMusic) == TRUE;
            }
            set
            {
                PlayerPrefs.SetInt(Keys.MuteMusic, value ? TRUE : FALSE);
            }
        }

        public bool ShowInGameHints
        {
            get
            {
                return PlayerPrefs.GetInt(Keys.ShowInGameHints) == TRUE;
            }
            set
            {
                PlayerPrefs.SetInt(Keys.ShowInGameHints, value ? TRUE : FALSE);
            }
        }

        public SettingsManager()
        {
            if (!PlayerPrefs.HasKey(Keys.Difficulty)
                || !PlayerPrefs.HasKey(Keys.ZoomSpeedLevel)
                || !PlayerPrefs.HasKey(Keys.ScrollSpeed)
                || !PlayerPrefs.HasKey(Keys.MuteMusic)
                || !PlayerPrefs.HasKey(Keys.ShowInGameHints))
            {
                CreateSettings();
            }
        }

        private void CreateSettings()
        {
            AIDifficulty = Difficulty.Normal;
            ZoomSpeedLevel = DEFAULT_ZOOM_SPEED_LEVEL;
			ScrollSpeedLevel = DEFAULT_SCROLL_SPEED_LEVEL;
            MuteMusic = false;
            ShowInGameHints = true;

            Save();
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}
