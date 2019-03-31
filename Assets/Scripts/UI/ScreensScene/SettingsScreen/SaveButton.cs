using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SaveButton : Togglable, IPointerClickHandler
    {
        private ISettingsManager _settingsManager;
        private IDifficultyDropdown _difficultyDropdown;
        private IBroadcastingProperty<float> _zoomSpeed;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(
            ISettingsManager settingsManager, 
            IDifficultyDropdown difficultyDropdown,
            IBroadcastingProperty<float> zoomSpeed)
        {
            Helper.AssertIsNotNull(settingsManager, difficultyDropdown, zoomSpeed);

            _settingsManager = settingsManager;
            _difficultyDropdown = difficultyDropdown;
            _zoomSpeed = zoomSpeed;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _difficultyDropdown.DifficultyChanged += _difficultyDropdown_DifficultyChanged;
            _zoomSpeed.ValueChanged += _zoomSpeed_ValueChanged;

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

        public void OnPointerClick(PointerEventData eventData)
        {
            Assert.IsTrue(ShouldBeEnabled());

            _settingsManager.AIDifficulty = _difficultyDropdown.Difficulty;
            _settingsManager.ZoomSpeed = _zoomSpeed.Value;
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
                || _zoomSpeed.Value != _settingsManager.ZoomSpeed;
        }
    }
}