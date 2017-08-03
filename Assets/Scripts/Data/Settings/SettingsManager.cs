using System;
using UnityEngine;

namespace BattleCruisers.Data.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private static class Keys
        {
            public static string Difficulty = "Difficulty";
        }

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
            Save();
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}