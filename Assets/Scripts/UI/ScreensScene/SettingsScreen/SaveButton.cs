using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SaveButton : ElementWithClickSound
    {
        private IScreensSceneGod _screensSceneGod;
        private ISettingsManager _settingsManager;
        private IDifficultyDropdown _difficultyDropdown;
        private IBroadcastingProperty<int> _zoomSpeedLevel, _scrollSpeedLevel;
        private IBroadcastingProperty<float> _musicVolume, _effectVolume, _masterVolume;
        private IBroadcastingProperty<bool> _showInGameHints;
        private IHotkeysPanel _hotkeysPanel;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            IDismissableEmitter parent,
            IScreensSceneGod screensSceneGod,
            ISettingsManager settingsManager, 
            IDifficultyDropdown difficultyDropdown,
            IBroadcastingProperty<int> zoomSpeedLevel,
            IBroadcastingProperty<int> scrollSpeedLevel,
            IBroadcastingProperty<float> musicVolume,
            IBroadcastingProperty<float> effectVolume,
            IBroadcastingProperty<float> masterVolume,
            IBroadcastingProperty<bool> showInGameHints,
            IHotkeysPanel hotkeysPanel)
        {
            base.Initialise(soundPlayer, parent: parent);

            Helper.AssertIsNotNull(screensSceneGod, settingsManager, difficultyDropdown, zoomSpeedLevel, scrollSpeedLevel, musicVolume, effectVolume, showInGameHints, hotkeysPanel);

            _screensSceneGod = screensSceneGod;
            _settingsManager = settingsManager;
            _difficultyDropdown = difficultyDropdown;
            _zoomSpeedLevel = zoomSpeedLevel;
            _scrollSpeedLevel = scrollSpeedLevel;
            _musicVolume = musicVolume;
            _effectVolume = effectVolume;
            _masterVolume = masterVolume;
            _showInGameHints = showInGameHints;
            _hotkeysPanel = hotkeysPanel;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _difficultyDropdown.DifficultyChanged += (sender, e) => UpdateEnabledStatus();
            _zoomSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _scrollSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _musicVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _effectVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _masterVolume.ValueChanged += (sender, e) => UpdateEnabledStatus();
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
            _settingsManager.MasterVolume = _masterVolume.Value;
            _settingsManager.MusicVolume = _musicVolume.Value;
            _settingsManager.EffectVolume = _effectVolume.Value;
            
            _settingsManager.ShowInGameHints = _showInGameHints.Value;
            _settingsManager.Save();

            UpdateEnabledStatus();

            //_screensSceneGod.GoToHomeScreen();
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
                || _effectVolume.Value != _settingsManager.EffectVolume
                || _masterVolume.Value != _settingsManager.MasterVolume
                || _showInGameHints.Value != _settingsManager.ShowInGameHints
                || _hotkeysPanel.IsDirty.Value;
        }
    }
}