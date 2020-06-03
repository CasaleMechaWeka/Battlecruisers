using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
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
        private IDifficultyDropdown _difficultyDropdown;
        private IBroadcastingProperty<int> _zoomSpeedLevel, _scrollSpeedLevel;
        private IBroadcastingProperty<bool> _muteMusic;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISoundPlayer soundPlayer,
            IScreensSceneGod screensSceneGod,
            ISettingsManager settingsManager, 
            IDifficultyDropdown difficultyDropdown,
            IBroadcastingProperty<int> zoomSpeedLevel,
            IBroadcastingProperty<int> scrollSpeedLevel,
            IBroadcastingProperty<bool> muteMusic)
        {
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(screensSceneGod, settingsManager, difficultyDropdown, zoomSpeedLevel, scrollSpeedLevel, muteMusic);

            _screensSceneGod = screensSceneGod;
            _settingsManager = settingsManager;
            _difficultyDropdown = difficultyDropdown;
            _zoomSpeedLevel = zoomSpeedLevel;
            _scrollSpeedLevel = scrollSpeedLevel;
            _muteMusic = muteMusic;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _difficultyDropdown.DifficultyChanged += (sender, e) => UpdateEnabledStatus();
            _zoomSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _scrollSpeedLevel.ValueChanged += (sender, e) => UpdateEnabledStatus();
            _muteMusic.ValueChanged += (sender, e) => UpdateEnabledStatus();

            UpdateEnabledStatus();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            Assert.IsTrue(ShouldBeEnabled());

            _settingsManager.AIDifficulty = _difficultyDropdown.Difficulty;
            _settingsManager.ZoomSpeedLevel = _zoomSpeedLevel.Value;
            _settingsManager.ScrollSpeedLevel = _scrollSpeedLevel.Value;
            _settingsManager.MuteMusic = _muteMusic.Value;
            _settingsManager.Save();

            UpdateEnabledStatus();

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
                || _muteMusic.Value != _settingsManager.MuteMusic;
        }
    }
}