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

        public Panel gameSettingsPanel;
        public HotkeysPanel hotkeysPanel;
        public SettingsTabButton gameSettingsButton, hotkeysButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod, 
            ISingleSoundPlayer soundPlayer, 
            ISettingsManager settingsManager,
            IMusicPlayer musicPlayer,
            IHotkeysModel hotkeysModel)
		{
			base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(difficultyDropdown, zoomSlider, scrollSlider, muteMusicToggle, muteVoicesToggle, showInGameHintsToggle, cancelButton);
            Helper.AssertIsNotNull(gameSettingsPanel, hotkeysPanel, gameSettingsButton, hotkeysButton);
            Helper.AssertIsNotNull(soundPlayer, screensSceneGod, settingsManager, musicPlayer, hotkeysModel);

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

            hotkeysPanel.Initialise(hotkeysModel);

            SaveButton saveButton = GetComponentInChildren<SaveButton>();
            Assert.IsNotNull(saveButton);
            saveButton
                .Initialise(
                    soundPlayer,
                    this,
                    screensSceneGod,
                    settingsManager,
                    musicPlayer,
                    difficultyDropdown,
                    zoomSlider.SliderValue,
                    scrollSlider.SliderValue,
                    muteMusicToggle.IsChecked,
                    muteVoicesToggle.IsChecked,
                    showInGameHintsToggle.IsChecked,
                    hotkeysPanel);

            cancelButton.Initialise(soundPlayer, this);

            gameSettingsButton.Initialise(soundPlayer, this, ShowGameSettings);
            hotkeysButton.Initialise(soundPlayer, this, ShowHotkeys);
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

        public override void OnDismissing()
        {
            base.OnDismissing();
            hotkeysPanel.Reset();
        }
    }
}
