using System;
using System.Collections.Generic;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class SettingsScreenController : ScreenController
    {
        private ISettingsManager _settingsManager;

		public Dropdown difficultyDropdown;
		
		public void Initialise(IScreensSceneGod screensSceneGod, ISettingsManager settingsManager)
		{
			base.Initialise(screensSceneGod);

            difficultyDropdown.AddOptions(CreateDropdownOptions());

			difficultyDropdown.onValueChanged.AddListener(OnDropdownChange);

            //settingsManager.
		}

        private List<Dropdown.OptionData> CreateDropdownOptions()
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
			{
                options.Add(new Dropdown.OptionData(difficulty.ToString()));
			}

            return options;
        }

        private void OnDropdownChange(int newIndex)
        {
            Debug.Log("newIndex: " + newIndex);


        }
	}
}
