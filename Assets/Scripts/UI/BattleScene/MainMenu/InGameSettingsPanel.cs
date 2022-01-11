using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using System;

namespace BattleCruisers.UI.BattleScene.MainMenu
{
    public class InGameSettingsPanel : Panel
    {
        private ISettingsManager _settingsManager;

        public InGameSaveButton saveButton;
        public CanvasGroupButton cancelButton;
        public FloatSliderController musicVolumeSlider, effectVolumeSlider;
        public SliderController zoomSlider, scrollSlider;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IMainMenuManager mainMenuManager,
            ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(saveButton, cancelButton, musicVolumeSlider, effectVolumeSlider);
            Helper.AssertIsNotNull(soundPlayer, mainMenuManager, settingsManager);

            _settingsManager = settingsManager;

            IRange<float> musicVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            musicVolumeSlider.Initialise(settingsManager.MusicVolume, musicVolumeRange);

            IRange<float> effectVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            effectVolumeSlider.Initialise(settingsManager.EffectVolume, effectVolumeRange);

            IRange<int> zoomlLevelRange = new Range<int>(SettingsModel.MIN_ZOOM_SPEED_LEVEL, SettingsModel.MAX_ZOOM_SPEED_LEVEL);
            zoomSlider.Initialise(_settingsManager.ZoomSpeedLevel, zoomlLevelRange);

            IRange<int> scrollLevelRange = new Range<int>(SettingsModel.MIN_SCROLL_SPEED_LEVEL, SettingsModel.MAX_SCROLL_SPEED_LEVEL);
            scrollSlider.Initialise(_settingsManager.ScrollSpeedLevel, scrollLevelRange);

            saveButton
                .Initialise(
                    soundPlayer,
                    mainMenuManager,
                    settingsManager,
                    musicVolumeSlider.SliderValue,
                    effectVolumeSlider.SliderValue,
                    zoomSlider.SliderValue,
                    scrollSlider.SliderValue);

            cancelButton.Initialise(soundPlayer, mainMenuManager.DismissMenu);

            mainMenuManager.Dismissed += MainMenuManager_Dismissed;
        }

        private void MainMenuManager_Dismissed(object sender, EventArgs e)
        {
            // Discard unsaved changes
            musicVolumeSlider.ResetToDefaults(_settingsManager.MusicVolume);
            effectVolumeSlider.ResetToDefaults(_settingsManager.EffectVolume);
            zoomSlider.ResetToDefaults(_settingsManager.ZoomSpeedLevel);
            scrollSlider.ResetToDefaults(_settingsManager.ScrollSpeedLevel);
        }
    }
}