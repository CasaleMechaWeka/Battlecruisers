using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SettingsScreenController : ScreenController
    {
        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IScreensSceneGod screensSceneGod, 
            ISettingsManager settingsManager,
            IMusicPlayer musicPlayer)
		{
			base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(settingsManager, musicPlayer);

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

            IRange<int> zoomlLevelRange = new Range<int>(SettingsManager.MIN_ZOOM_SPEED_LEVEL, SettingsManager.MAX_ZOOM_SPEED_LEVEL);
            SliderController zoomSlider = transform.FindNamedComponent<SliderController>("SettingsContainer/ZoomSpeedRow/Slider");
            zoomSlider.Initialise(settingsManager.ZoomSpeedLevel, zoomlLevelRange);

            IRange<int> scrollLevelRange = new Range<int>(SettingsManager.MIN_SCROLL_SPEED_LEVEL, SettingsManager.MAX_SCROLL_SPEED_LEVEL);
            SliderController scrollSlider = transform.FindNamedComponent<SliderController>("SettingsContainer/ScrollSpeedRow/Slider");
            scrollSlider.Initialise(settingsManager.ScrollSpeedLevel, scrollLevelRange);

            ToggleController muteMusicToggle = GetComponentInChildren<ToggleController>();
            muteMusicToggle.Initialise(settingsManager.MuteMusic);

            SaveButton saveButton = GetComponentInChildren<SaveButton>();
            Assert.IsNotNull(saveButton);
            saveButton
                .Initialise(
                    soundPlayer,
                    screensSceneGod,
                    settingsManager,
                    musicPlayer,
                    difficultyDropdown,
                    zoomSlider.SliderValue,
                    scrollSlider.SliderValue,
                    muteMusicToggle.IsChecked,
                    this);

            CancelButton cancelButton = GetComponentInChildren<CancelButton>();
            Assert.IsNotNull(cancelButton);
            cancelButton.Initialise(_soundPlayer, screensSceneGod, this);
		}
	}
}
