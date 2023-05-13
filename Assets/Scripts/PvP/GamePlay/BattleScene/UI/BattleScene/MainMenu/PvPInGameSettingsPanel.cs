using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Panels;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.ScreensScene.SettingsScreen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu
{
    public class PvPInGameSettingsPanel : PvPPanel
    {
        private ISettingsManager _settingsManager;

        public PvPInGameSaveButton saveButton;
        public PvPCanvasGroupButton cancelButton;
        public PvPFloatSliderController masterVolumeSlider, musicVolumeSlider, effectVolumeSlider, alertVolumeSlider, interfaceVolumeSlider, ambientVolumeSlider;
        public PvPToggleController showToolTipsToggle;
        public PvPSliderController zoomSlider, scrollSlider;

        public void Initialise(
            IPvPSingleSoundPlayer soundPlayer,
            IPvPMainMenuManager mainMenuManager,
            ISettingsManager settingsManager)
        {
            PvPHelper.AssertIsNotNull(saveButton, cancelButton, musicVolumeSlider, effectVolumeSlider);
            PvPHelper.AssertIsNotNull(soundPlayer, mainMenuManager, settingsManager);

            _settingsManager = settingsManager;

            IPvPRange<float> masterVolumeRange = new PvPRange<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            masterVolumeSlider.Initialise(settingsManager.MasterVolume, masterVolumeRange);

            IPvPRange<float> musicVolumeRange = new PvPRange<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            musicVolumeSlider.Initialise(settingsManager.MusicVolume, musicVolumeRange);

            IPvPRange<float> effectVolumeRange = new PvPRange<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            effectVolumeSlider.Initialise(settingsManager.EffectVolume, effectVolumeRange);

            IPvPRange<float> alertVolumeRange = new PvPRange<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            alertVolumeSlider.Initialise(settingsManager.AlertVolume, alertVolumeRange);

            IPvPRange<float> interfaceVolumeRange = new PvPRange<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            interfaceVolumeSlider.Initialise(settingsManager.InterfaceVolume, interfaceVolumeRange);

            IPvPRange<float> ambientVolumeRange = new PvPRange<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            ambientVolumeSlider.Initialise(settingsManager.AmbientVolume, ambientVolumeRange);

            IPvPRange<int> zoomlLevelRange = new PvPRange<int>(SettingsModel.MIN_ZOOM_SPEED_LEVEL, SettingsModel.MAX_ZOOM_SPEED_LEVEL);
            zoomSlider.Initialise(_settingsManager.ZoomSpeedLevel, zoomlLevelRange);

            IPvPRange<int> scrollLevelRange = new PvPRange<int>(SettingsModel.MIN_SCROLL_SPEED_LEVEL, SettingsModel.MAX_SCROLL_SPEED_LEVEL);
            scrollSlider.Initialise(_settingsManager.ScrollSpeedLevel, scrollLevelRange);

            showToolTipsToggle.Initialise(_settingsManager.ShowToolTips);


            saveButton
                .Initialise(
                    soundPlayer,
                    mainMenuManager,
                    settingsManager,
                    musicVolumeSlider.SliderValue,
                    effectVolumeSlider.SliderValue,
                    zoomSlider.SliderValue,
                    scrollSlider.SliderValue,
                    masterVolumeSlider.SliderValue,
                    alertVolumeSlider.SliderValue,
                    interfaceVolumeSlider.SliderValue,
                    ambientVolumeSlider.SliderValue,
                    showToolTipsToggle.IsChecked);

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
            masterVolumeSlider.ResetToDefaults(_settingsManager.MasterVolume);
            alertVolumeSlider.ResetToDefaults(_settingsManager.AlertVolume);
            interfaceVolumeSlider.ResetToDefaults(_settingsManager.InterfaceVolume);
            ambientVolumeSlider.ResetToDefaults(_settingsManager.AmbientVolume);
            showToolTipsToggle.ResetToDefaults(_settingsManager.ShowToolTips);
        }
    }
}