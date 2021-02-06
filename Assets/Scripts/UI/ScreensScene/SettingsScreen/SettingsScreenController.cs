using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SettingsScreenController : ScreenController
    {
        private ISettingsManager _settingsManager;

        public DifficultyDropdown difficultyDropdown;
        public SliderController zoomSlider, scrollSlider;
        public FloatSliderController musicVolumeSlider, effectVolumeSlider;
        public ToggleController showInGameHintsToggle;
        public SaveButton saveButton;
        public CancelButton cancelButton;
        public CanvasGroupButton resetHotkeysButton;

        public Panel gameSettingsPanel;
        public HotkeysPanel hotkeysPanel;
        public SettingsTabButton gameSettingsButton, hotkeysButton;

        public bool showGameSettingsFirst = true;

        public void Initialise(
            IScreensSceneGod screensSceneGod, 
            ISingleSoundPlayer soundPlayer, 
            ISettingsManager settingsManager,
            IMusicPlayer musicPlayer,
            IHotkeysModel hotkeysModel)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(difficultyDropdown, zoomSlider, scrollSlider, musicVolumeSlider, effectVolumeSlider, showInGameHintsToggle, saveButton, cancelButton, resetHotkeysButton);
            Helper.AssertIsNotNull(gameSettingsPanel, hotkeysPanel, gameSettingsButton, hotkeysButton);
            Helper.AssertIsNotNull(soundPlayer, screensSceneGod, settingsManager, musicPlayer, hotkeysModel);

            _settingsManager = settingsManager;

            // Scroll speed used to be 0.1 - 3.9 instead of 1 - 9.  Hence, reset :)
            if (_settingsManager.ScrollSpeedLevel < SettingsModel.MIN_SCROLL_SPEED_LEVEL
                || _settingsManager.ScrollSpeedLevel > SettingsModel.MAX_SCROLL_SPEED_LEVEL)
            {
                _settingsManager.ScrollSpeedLevel = SettingsModel.DEFAULT_SCROLL_SPEED_LEVEL;
                _settingsManager.Save();
            }

            difficultyDropdown.Initialise(_settingsManager.AIDifficulty);

            IRange<int> zoomlLevelRange = new Range<int>(SettingsModel.MIN_ZOOM_SPEED_LEVEL, SettingsModel.MAX_ZOOM_SPEED_LEVEL);
            zoomSlider.Initialise(_settingsManager.ZoomSpeedLevel, zoomlLevelRange);

            IRange<int> scrollLevelRange = new Range<int>(SettingsModel.MIN_SCROLL_SPEED_LEVEL, SettingsModel.MAX_SCROLL_SPEED_LEVEL);
            scrollSlider.Initialise(_settingsManager.ScrollSpeedLevel, scrollLevelRange);

            IRange<float> musicVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            musicVolumeSlider.Initialise(_settingsManager.MusicVolume, musicVolumeRange);

            IRange<float> effectVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            effectVolumeSlider.Initialise(_settingsManager.EffectVolume, effectVolumeRange);

            showInGameHintsToggle.Initialise(_settingsManager.ShowInGameHints);

            hotkeysPanel.Initialise(hotkeysModel);

            saveButton
                .Initialise(
                    soundPlayer,
                    this,
                    screensSceneGod,
                    _settingsManager,
                    musicPlayer,
                    difficultyDropdown,
                    zoomSlider.SliderValue,
                    scrollSlider.SliderValue,
                    musicVolumeSlider.SliderValue,
                    effectVolumeSlider.SliderValue,
                    showInGameHintsToggle.IsChecked,
                    hotkeysPanel);

            cancelButton.Initialise(soundPlayer, this);
            resetHotkeysButton.Initialise(soundPlayer, hotkeysPanel.ResetToDefaults);

            gameSettingsButton.Initialise(soundPlayer, ShowGameSettings, this);
            hotkeysButton.Initialise(soundPlayer, ShowHotkeys, this);

            ShowTab();
        }

        private void ShowTab()
        {
            if (SystemInfoBC.Instance.IsHandheld)
            {
                // There are no hotkeys for handheld devices
                ShowGameSettings();

                hotkeysPanel.Hide();
                gameSettingsButton.IsVisible = false;
                hotkeysButton.IsVisible = false;
                return;
            }

            if (showGameSettingsFirst)
            {
                ShowGameSettings();
            }
            else
            {
                ShowHotkeys();
            }
        }

        public override void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }

        public void ShowGameSettings()
        {
            hotkeysPanel.Hide();
            hotkeysButton.IsSelected = false;
            resetHotkeysButton.IsVisible = false;

            gameSettingsPanel.Show();
            gameSettingsButton.IsSelected = true;
        }

        public void ShowHotkeys()
        {
            gameSettingsPanel.Hide();
            gameSettingsButton.IsSelected = false;

            hotkeysPanel.Show();
            hotkeysButton.IsSelected = true;
            resetHotkeysButton.IsVisible = true;
        }

        public override void OnDismissing()
        {
            base.OnDismissing();

            hotkeysPanel.ResetToSavedState();
            difficultyDropdown.ResetToDefaults(_settingsManager.AIDifficulty);
            zoomSlider.ResetToDefaults(_settingsManager.ZoomSpeedLevel);
            scrollSlider.ResetToDefaults(_settingsManager.ScrollSpeedLevel);
            musicVolumeSlider.ResetToDefaults(_settingsManager.MusicVolume);
            effectVolumeSlider.ResetToDefaults(_settingsManager.EffectVolume);
            showInGameHintsToggle.ResetToDefaults(_settingsManager.ShowInGameHints);
        }
    }
}
