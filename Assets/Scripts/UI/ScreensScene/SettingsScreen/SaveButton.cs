using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SaveButton : ClickableTogglable
    {
        private ISettingsManager _settingsManager;
        private IDifficultyDropdown _difficultyDropdown;
        // FELIX  Rename, add level :)
        private IBroadcastingProperty<int> _zoomSpeed, _scrollSpeedLevel;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISettingsManager settingsManager, 
            IDifficultyDropdown difficultyDropdown,
            IBroadcastingProperty<int> zoomSpeed,
            IBroadcastingProperty<int> scrollSpeedLevel)
        {
            base.Initialise();

            Helper.AssertIsNotNull(settingsManager, difficultyDropdown, zoomSpeed, scrollSpeedLevel);

            _settingsManager = settingsManager;
            _difficultyDropdown = difficultyDropdown;
            _zoomSpeed = zoomSpeed;
            _scrollSpeedLevel = scrollSpeedLevel;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _difficultyDropdown.DifficultyChanged += _difficultyDropdown_DifficultyChanged;
            _zoomSpeed.ValueChanged += _zoomSpeed_ValueChanged;
            _scrollSpeedLevel.ValueChanged += _scrollSpeedLevel_ValueChanged;

            UpdateEnabledStatus();
        }

        private void _difficultyDropdown_DifficultyChanged(object sender, EventArgs e)
        {
            UpdateEnabledStatus();
        }

        private void _zoomSpeed_ValueChanged(object sender, EventArgs e)
        {
            UpdateEnabledStatus();
        }

        private void _scrollSpeedLevel_ValueChanged(object sender, EventArgs e)
        {
            UpdateEnabledStatus();
        }

        protected override void OnClicked()
        {
            Assert.IsTrue(ShouldBeEnabled());

            _settingsManager.AIDifficulty = _difficultyDropdown.Difficulty;
            _settingsManager.ZoomSpeedLevel = _zoomSpeed.Value;
            _settingsManager.ScrollSpeedLevel = _scrollSpeedLevel.Value;
            _settingsManager.Save();

            UpdateEnabledStatus();
        }

        private void UpdateEnabledStatus()
        {
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            return
                _difficultyDropdown.Difficulty != _settingsManager.AIDifficulty
                || _zoomSpeed.Value != _settingsManager.ZoomSpeedLevel
                || _scrollSpeedLevel.Value != _settingsManager.ScrollSpeedLevel;
        }
    }
}