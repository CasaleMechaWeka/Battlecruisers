using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SaveButton : ElementWithClickSound
    {
        private IScreensSceneGod _screensSceneGod;
        private ISettingsManager _settingsManager;
        private IMusicPlayer _musicPlayer;
        private IDifficultyDropdown _difficultyDropdown;
        private IBroadcastingProperty<int> _zoomSpeedLevel, _scrollSpeedLevel;
        private IBroadcastingProperty<float> _musicVolume;
        private IBroadcastingProperty<bool> _muteVoices, _showInGameHints;
        private IHotkeysPanel _hotkeysPanel;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IDismissableEmitter parent,
            IScreensSceneGod screensSceneGod,
            ISettingsManager settingsManager, 
            IMusicPlayer musicPlayer,
            IDifficultyDropdown difficultyDropdown,
            IBroadcastingProperty<int> zoomSpeedLevel,
            IBroadcastingProperty<int> scrollSpeedLevel,
            IBroadcastingProperty<float> musicVolume,
            IBroadcastingProperty<bool> muteVoices,
            IBroadcastingProperty<bool> showInGameHints,
            IHotkeysPanel hotkeysPanel)
        {
            base.Initialise(soundPlayer, parent: parent);

            Helper.AssertIsNotNull(screensSceneGod, settingsManager, musicPlayer, difficultyDropdown, zoomSpeedLevel, scrollSpeedLevel, musicVolume, muteVoices, showInGameHints, hotkeysPanel);

            _screensSceneGod = screensSceneGod;
            _settingsManager = settingsManager;
            _musicPlayer = musicPlayer;
            _difficultyDropdown = difficultyDropdown;
            _zoomSpeedLevel = zoomSpeedLevel;
            _scrollSpeedLevel = scrollSpeedLevel;
            _musicVolume = musicVolume;
            _muteVoices = muteVoices;
            _showInGameHints = showInGameHints;
            _hotkeysPanel = hotkeysPanel;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _difficultyDropdown.DifficultyChanged += (sender, e) => UpdateEnabledStatus();
            _zoomSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _scrollSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _musicVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _muteVoices.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _showInGameHints.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _hotkeysPanel.IsDirty.ValueChanged += (sender, e) => UpdateEnabledStatus();

            UpdateEnabledStatus();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            Assert.IsTrue(ShouldBeEnabled());

            _hotkeysPanel.UpdateHokeysModel();

            _settingsManager.AIDifficulty = _difficultyDropdown.Difficulty;
            _settingsManager.ZoomSpeedLevel = _zoomSpeedLevel.Value;
            _settingsManager.ScrollSpeedLevel = _scrollSpeedLevel.Value;
            _settingsManager.MusicVolume = _musicVolume.Value;
            _settingsManager.MuteVoices = _muteVoices.Value;
            _settingsManager.ShowInGameHints = _showInGameHints.Value;
            _settingsManager.Save();

            UpdateEnabledStatus();

            _musicPlayer.Volume = _settingsManager.MusicVolume;

            _screensSceneGod.GoToHomeScreen();
        }

        private void UpdateEnabledStatus()
        {
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            return
                _difficultyDropdown.Difficulty != _settingsManager.AIDifficulty
                || _zoomSpeedLevel.Value != _settingsManager.ZoomSpeedLevel
                || _scrollSpeedLevel.Value != _settingsManager.ScrollSpeedLevel
                || _musicVolume.Value != _settingsManager.MusicVolume
                || _muteVoices.Value != _settingsManager.MuteVoices
                || _showInGameHints.Value != _settingsManager.ShowInGameHints
                || _hotkeysPanel.IsDirty.Value;
        }
    }
}