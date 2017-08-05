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

            difficultyDropdown.onValueChanged.AddListener(OnDropdownChange);
		}

        private void OnDropdownChange(int newIndex)
        {
            Debug.Log("newIndex: " + newIndex);
        }
	}
}
