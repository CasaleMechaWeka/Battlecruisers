using System;
using System.Collections.Generic;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class SettingsScreenController : ScreenController
    {
        private ISettingsManager _settingsManager;
        private IList<Difficulty> _difficulties;

		public Dropdown difficultyDropdown;
		
		public void Initialise(IScreensSceneGod screensSceneGod, ISettingsManager settingsManager)
		{
			base.Initialise(screensSceneGod);

            Assert.IsNotNull(settingsManager);
            _settingsManager = settingsManager;

            SetupDropdown();
		}

        private void SetupDropdown()
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            _difficulties = new List<Difficulty>();
            int currentIndex = 0;

            Difficulty[] difficulties = (Difficulty[])Enum.GetValues(typeof(Difficulty));

			for (int i = 0; i < difficulties.Length; ++i)
            {
                Difficulty difficulty = difficulties[i];

                options.Add(new Dropdown.OptionData(difficulty.ToString()));
                _difficulties.Add(difficulty);

                if (difficulty == _settingsManager.AIDifficulty)
                {
                    currentIndex = i;
                }
            }

            difficultyDropdown.AddOptions(options);
            difficultyDropdown.value = currentIndex;
        }

        public void Save()
        {
            Assert.IsTrue(difficultyDropdown.value < _difficulties.Count);
            _settingsManager.AIDifficulty = _difficulties[difficultyDropdown.value];

            _settingsManager.Save();

            _screensSceneGod.GoToHomeScreen();
        }

        public void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }
	}
}
