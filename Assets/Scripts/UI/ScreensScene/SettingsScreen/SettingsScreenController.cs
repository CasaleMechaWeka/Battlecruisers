using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SettingsScreenController : ScreenController
    {
        public DifficultyDropdown difficultyDropdown;
        public SliderController zoomSlider, scrollSlider;
        public ToggleController muteMusicToggle, muteVoicesToggle, showInGameHintsToggle;
        public CancelButton cancelButton;

        public Panel gameSettingsPanel, hotkeysPanel;
        public SettingsTabButton gameSettingsButton, hotkeysButton;

        // FELIX  TEMP
        public ActionButton tempButton;

        public void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IScreensSceneGod screensSceneGod, 
            ISettingsManager settingsManager,
            IMusicPlayer musicPlayer)
		{
			base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(difficultyDropdown, zoomSlider, scrollSlider, muteMusicToggle, muteVoicesToggle, showInGameHintsToggle, cancelButton);
            Helper.AssertIsNotNull(gameSettingsPanel, hotkeysPanel, gameSettingsButton, hotkeysButton);
            Helper.AssertIsNotNull(settingsManager, musicPlayer);

            // Scroll speed used to be 0.1 - 3.9 instead of 1 - 9.  Hence, reset :)
            if (settingsManager.ScrollSpeedLevel < SettingsModel.MIN_SCROLL_SPEED_LEVEL
                || settingsManager.ScrollSpeedLevel > SettingsModel.MAX_SCROLL_SPEED_LEVEL)
            {
                settingsManager.ScrollSpeedLevel = SettingsModel.DEFAULT_SCROLL_SPEED_LEVEL;
                settingsManager.Save();
            }

            difficultyDropdown.Initialise(settingsManager.AIDifficulty);

            IRange<int> zoomlLevelRange = new Range<int>(SettingsModel.MIN_ZOOM_SPEED_LEVEL, SettingsModel.MAX_ZOOM_SPEED_LEVEL);
            zoomSlider.Initialise(settingsManager.ZoomSpeedLevel, zoomlLevelRange);

            IRange<int> scrollLevelRange = new Range<int>(SettingsModel.MIN_SCROLL_SPEED_LEVEL, SettingsModel.MAX_SCROLL_SPEED_LEVEL);
            scrollSlider.Initialise(settingsManager.ScrollSpeedLevel, scrollLevelRange);

            muteMusicToggle.Initialise(settingsManager.MuteMusic);
            muteVoicesToggle.Initialise(settingsManager.MuteVoices);
            showInGameHintsToggle.Initialise(settingsManager.ShowInGameHints);

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
                    muteVoicesToggle.IsChecked,
                    showInGameHintsToggle.IsChecked,
                    this);

            cancelButton.Initialise(_soundPlayer, this);

            gameSettingsButton.Initialise(soundPlayer, this, ShowGameSettings);
            hotkeysButton.Initialise(soundPlayer, this, ShowHotkeys);

            tempButton.Initialise(soundPlayer, this, ShowGameSettings);
		}

        public override void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }

        public void ShowGameSettings()
        {
            hotkeysPanel.Hide();
            gameSettingsPanel.Show();
        }

        public void ShowHotkeys()
        {
            gameSettingsPanel.Hide();
            hotkeysPanel.Show();
        }
    }
}
