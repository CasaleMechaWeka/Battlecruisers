using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SettingsScreenController : ScreenController
    {
        private ISettingsManager _settingsManager;

        public DifficultyDropdown difficultyDropdown;
        public LanguageDropdown languageDropdown;
        public ResolutionDropdown resolutionDropdown;
        public SliderController zoomSlider, scrollSlider;
        public FloatSliderController musicVolumeSlider, effectVolumeSlider, masterVolumeSlider, alertVolumeSlider, interfaceVolumeSlider, ambientVolumeSlider;
        public ToggleController showInGameHintsToggle, showToolTipsToggle, altDroneSoundsToggle, fullScreenToggle, VSyncToggle;
        public SaveButton saveButton;
        public CancelButton cancelButton;
        public CanvasGroupButton resetHotkeysButton;

        public Panel gameSettingsPanel, audioPanel, videoPanel;
        public HotkeysPanel hotkeysPanel;
        public SettingsTabButton gameSettingsButton, hotkeysButton, audioButton, languageButton;

        public bool showGameSettingsFirst = true;

        public void Initialise(
            IScreensSceneGod screensSceneGod, 
            ISingleSoundPlayer soundPlayer, 
            ISettingsManager settingsManager,
            IHotkeysModel hotkeysModel,
            ILocTable commonLocTable)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(difficultyDropdown, zoomSlider, scrollSlider, musicVolumeSlider, effectVolumeSlider, showInGameHintsToggle, saveButton, cancelButton, resetHotkeysButton);
            Helper.AssertIsNotNull(gameSettingsPanel, hotkeysPanel, gameSettingsButton, hotkeysButton, audioButton);
            Helper.AssertIsNotNull(soundPlayer, screensSceneGod, settingsManager, hotkeysModel, commonLocTable);

            _settingsManager = settingsManager;

            // Scroll speed used to be 0.1 - 3.9 instead of 1 - 9.  Hence, reset :)
            if (_settingsManager.ScrollSpeedLevel < SettingsModel.MIN_SCROLL_SPEED_LEVEL
                || _settingsManager.ScrollSpeedLevel > SettingsModel.MAX_SCROLL_SPEED_LEVEL)
            {
                _settingsManager.ScrollSpeedLevel = SettingsModel.DEFAULT_SCROLL_SPEED_LEVEL;
                _settingsManager.Save();
            }

            difficultyDropdown.Initialise(_settingsManager.AIDifficulty, commonLocTable);
            languageDropdown.Initialise(_settingsManager.Language, commonLocTable);
            resolutionDropdown.Initialise(_settingsManager.Resolution, commonLocTable);

            
            IRange<int> zoomlLevelRange = new Range<int>(SettingsModel.MIN_ZOOM_SPEED_LEVEL, SettingsModel.MAX_ZOOM_SPEED_LEVEL);
            zoomSlider.Initialise(_settingsManager.ZoomSpeedLevel, zoomlLevelRange);

            IRange<int> scrollLevelRange = new Range<int>(SettingsModel.MIN_SCROLL_SPEED_LEVEL, SettingsModel.MAX_SCROLL_SPEED_LEVEL);
            scrollSlider.Initialise(_settingsManager.ScrollSpeedLevel, scrollLevelRange);

            IRange<float> musicVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            musicVolumeSlider.Initialise(_settingsManager.MusicVolume, musicVolumeRange);

            IRange<float> effectVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            effectVolumeSlider.Initialise(_settingsManager.EffectVolume, effectVolumeRange);

            IRange<float> masterVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            masterVolumeSlider.Initialise(_settingsManager.MasterVolume, masterVolumeRange);

            IRange<float> alertVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            alertVolumeSlider.Initialise(_settingsManager.AlertVolume, alertVolumeRange);

            IRange<float> interfaceVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            interfaceVolumeSlider.Initialise(_settingsManager.InterfaceVolume, interfaceVolumeRange);

            IRange<float> ambientVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            ambientVolumeSlider.Initialise(_settingsManager.AmbientVolume, ambientVolumeRange);

            showInGameHintsToggle.Initialise(_settingsManager.ShowInGameHints);
            showToolTipsToggle.Initialise(_settingsManager.ShowToolTips);
            altDroneSoundsToggle.Initialise(_settingsManager.AltDroneSounds);
            fullScreenToggle.Initialise(_settingsManager.FullScreen);
            VSyncToggle.Initialise(_settingsManager.VSync);

            hotkeysPanel.Initialise(hotkeysModel);

            saveButton
                .Initialise(
                    soundPlayer,
                    this,
                    screensSceneGod,
                    _settingsManager,
                    difficultyDropdown,
                    languageDropdown,
                    resolutionDropdown,
                    zoomSlider.SliderValue,
                    scrollSlider.SliderValue,
                    musicVolumeSlider.SliderValue,
                    effectVolumeSlider.SliderValue,
                    masterVolumeSlider.SliderValue,
                    alertVolumeSlider.SliderValue,
                    interfaceVolumeSlider.SliderValue,
                    ambientVolumeSlider.SliderValue,
                    showInGameHintsToggle.IsChecked,
                    showToolTipsToggle.IsChecked,
                    altDroneSoundsToggle.IsChecked,
                    fullScreenToggle.IsChecked,
                    VSyncToggle.IsChecked,
                    hotkeysPanel);

            cancelButton.Initialise(soundPlayer, this);
            resetHotkeysButton.Initialise(soundPlayer, hotkeysPanel.ResetToDefaults);

            gameSettingsButton.Initialise(soundPlayer, ShowGameSettings, this);
            hotkeysButton.Initialise(soundPlayer, ShowHotkeys, this);
            audioButton.Initialise(soundPlayer, ShowAudioSettings, this);
            languageButton.Initialise(soundPlayer, ShowLanguageSettings, this);

            ShowTab();
        }

        private void ShowTab()
        {
            if (SystemInfoBC.Instance.IsHandheld)
            {
                // There are no hotkeys for handheld devices
                ShowGameSettings();

                hotkeysPanel.Hide();
                //gameSettingsButton.IsVisible = false;
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

            audioButton.IsSelected = false;
            audioPanel.Hide();
            languageButton.IsSelected = false;
            videoPanel.Hide();


            gameSettingsPanel.Show();
            gameSettingsButton.IsSelected = true;
        }

        public void ShowHotkeys()
        {
            gameSettingsPanel.Hide();
            gameSettingsButton.IsSelected = false;

            audioButton.IsSelected = false;
            audioPanel.Hide();

            languageButton.IsSelected = false;
            videoPanel.Hide();

            hotkeysPanel.Show();
            hotkeysButton.IsSelected = true;
            resetHotkeysButton.IsVisible = true;
        }

        public void ShowAudioSettings()
        {
            hotkeysPanel.Hide();
            hotkeysButton.IsSelected = false;
            resetHotkeysButton.IsVisible = false;
            gameSettingsPanel.Hide();
            gameSettingsButton.IsSelected = false;
            languageButton.IsSelected = false;
            videoPanel.Hide();

            audioButton.IsSelected = true;
            audioPanel.Show();
            

        }

        public void ShowLanguageSettings()
        {

            hotkeysPanel.Hide();
            hotkeysButton.IsSelected = false;
            resetHotkeysButton.IsVisible = false;
            gameSettingsPanel.Hide();
            gameSettingsButton.IsSelected = false;
            audioButton.IsSelected = false;
            audioPanel.Hide();
            
            languageButton.IsSelected = true;
            videoPanel.Show();

        }

        public override void OnDismissing()
        {
            base.OnDismissing();

            hotkeysPanel.ResetToSavedState();
            difficultyDropdown.ResetToDefaults(_settingsManager.AIDifficulty);
            languageDropdown.ResetToDefaults(_settingsManager.Language);
            zoomSlider.ResetToDefaults(_settingsManager.ZoomSpeedLevel);
            scrollSlider.ResetToDefaults(_settingsManager.ScrollSpeedLevel);
            masterVolumeSlider.ResetToDefaults(_settingsManager.MasterVolume);
            musicVolumeSlider.ResetToDefaults(_settingsManager.MusicVolume);
            effectVolumeSlider.ResetToDefaults(_settingsManager.EffectVolume);
            alertVolumeSlider.ResetToDefaults(_settingsManager.AlertVolume);
            interfaceVolumeSlider.ResetToDefaults(_settingsManager.InterfaceVolume);
            ambientVolumeSlider.ResetToDefaults(_settingsManager.AmbientVolume);
            showInGameHintsToggle.ResetToDefaults(_settingsManager.ShowInGameHints);
            showToolTipsToggle.ResetToDefaults(_settingsManager.ShowToolTips);
            VSyncToggle.ResetToDefaults(_settingsManager.VSync);
            fullScreenToggle.ResetToDefaults(_settingsManager.FullScreen);
            altDroneSoundsToggle.ResetToDefaults(_settingsManager.AltDroneSounds);
        }
    }
}
