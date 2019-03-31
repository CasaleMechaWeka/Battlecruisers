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

            DifficultyDropdown difficultyDropdown = GetComponentInChildren<DifficultyDropdown>();
            Assert.IsNotNull(difficultyDropdown);
            difficultyDropdown.Initialise(settingsManager.AIDifficulty);

            ZoomSlider zoomSlider = GetComponentInChildren<ZoomSlider>();
            Assert.IsNotNull(zoomSlider);
            zoomSlider.Initialise(settingsManager.ZoomSpeedLevel);

            SaveButton saveButton = GetComponentInChildren<SaveButton>();
            Assert.IsNotNull(saveButton);
            saveButton.Initialise(settingsManager, difficultyDropdown, zoomSlider.SliderValue);

            CancelButton cancelButton = GetComponentInChildren<CancelButton>();
            Assert.IsNotNull(cancelButton);
            cancelButton.Initialise(screensSceneGod);
		}
	}
}
