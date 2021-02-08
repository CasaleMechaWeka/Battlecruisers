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

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IMainMenuManager mainMenuManager,
            ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(saveButton, cancelButton, musicVolumeSlider, effectVolumeSlider);
            Helper.AssertIsNotNull(soundPlayer, mainMenuManager, settingsManager);

            _settingsManager = settingsManager;

            // FELIX  Avoid duplicate code with SettingsScreenController?
            IRange<float> musicVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            musicVolumeSlider.Initialise(settingsManager.MusicVolume, musicVolumeRange);

            IRange<float> effectVolumeRange = new Range<float>(SettingsModel.MIN_VOLUME, SettingsModel.MAX_VOLUME);
            effectVolumeSlider.Initialise(settingsManager.EffectVolume, effectVolumeRange);

            saveButton
                .Initialise(
                    soundPlayer,
                    mainMenuManager,
                    settingsManager,
                    musicVolumeSlider.SliderValue,
                    effectVolumeSlider.SliderValue);

            cancelButton.Initialise(soundPlayer, mainMenuManager.DismissMenu);

            mainMenuManager.Dismissed += MainMenuManager_Dismissed;
        }

        private void MainMenuManager_Dismissed(object sender, EventArgs e)
        {
            // Discard unsaved changes
            musicVolumeSlider.ResetToDefaults(_settingsManager.MusicVolume);
            effectVolumeSlider.ResetToDefaults(_settingsManager.EffectVolume);
        }
    }
}