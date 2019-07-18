using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SettingsScreenController : ScreenController
    {
        public void Initialise(IScreensSceneGod screensSceneGod, ISettingsManager settingsManager)
		{
			base.Initialise(screensSceneGod);

            Assert.IsNotNull(settingsManager);

            // Scroll speed used to be 0.1 - 3.9 instead of 1 - 9.  Hence, reset :)
            if (settingsManager.ScrollSpeedLevel < SettingsManager.MIN_SCROLL_SPEED_LEVEL
                || settingsManager.ScrollSpeedLevel > SettingsManager.MAX_SCROLL_SPEED_LEVEL)
            {
                settingsManager.ScrollSpeedLevel = SettingsManager.DEFAULT_SCROLL_SPEED_LEVEL;
                settingsManager.Save();
            }

            DifficultyDropdown difficultyDropdown = GetComponentInChildren<DifficultyDropdown>();
            Assert.IsNotNull(difficultyDropdown);
            difficultyDropdown.Initialise(settingsManager.AIDifficulty);

            ZoomSlider zoomSlider = GetComponentInChildren<ZoomSlider>();
            Assert.IsNotNull(zoomSlider);
            zoomSlider.Initialise(settingsManager.ZoomSpeedLevel);

            ScrollSlider scrollSlider = GetComponentInChildren<ScrollSlider>();
            Assert.IsNotNull(scrollSlider);
            scrollSlider.Initialise(settingsManager.ScrollSpeedLevel);

            SaveButton saveButton = GetComponentInChildren<SaveButton>();
            Assert.IsNotNull(saveButton);
            saveButton.Initialise(settingsManager, difficultyDropdown, zoomSlider.SliderValue, scrollSlider.SliderValue);

            CancelButton cancelButton = GetComponentInChildren<CancelButton>();
            Assert.IsNotNull(cancelButton);
            cancelButton.Initialise(screensSceneGod);
		}
	}
}
