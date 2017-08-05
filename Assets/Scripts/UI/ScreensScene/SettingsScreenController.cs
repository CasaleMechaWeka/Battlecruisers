using System;
using System.Collections.Generic;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    // FELIX  Select right difficulty for dropdown :P
    public class SettingsScreenController : ScreenController
    {
        private ISettingsManager _settingsManager;
        private IList<Difficulty> _difficulties;

		public Dropdown difficultyDropdown;
		
		public void Initialise(IScreensSceneGod screensSceneGod, ISettingsManager settingsManager)
		{
			base.Initialise(screensSceneGod);

            difficultyDropdown.AddOptions(CreateDropdownOptions());

			difficultyDropdown.onValueChanged.AddListener(OnDropdownChange);
		}

        private List<Dropdown.OptionData> CreateDropdownOptions()
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            _difficulties = new List<Difficulty>();

            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
			{
                options.Add(new Dropdown.OptionData(difficulty.ToString()));
                _difficulties.Add(difficulty);
			}

            return options;
        }

        private void OnDropdownChange(int newIndex)
        {
            Assert.IsTrue(newIndex < _difficulties.Count);
            _settingsManager.AIDifficulty = _difficulties[newIndex];
        }
	}
}
