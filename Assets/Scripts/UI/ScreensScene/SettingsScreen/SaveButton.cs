using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SaveButton : Togglable, IPointerClickHandler
    {
        private IDifficultyDropdown _difficultyDropdown;
        private ISettingsManager _settingsManager;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(IDifficultyDropdown difficultyDropdown, ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(difficultyDropdown, settingsManager);

            _difficultyDropdown = difficultyDropdown;
            _settingsManager = settingsManager;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _difficultyDropdown.DifficultyChanged += _difficultyDropdown_DifficultyChanged;

            UpdateEnabledStatus();
        }

        private void _difficultyDropdown_DifficultyChanged(object sender, EventArgs e)
        {
            UpdateEnabledStatus();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Assert.IsTrue(ShouldBeEnabled());

            _settingsManager.AIDifficulty = _difficultyDropdown.Difficulty;
            _settingsManager.Save();

            UpdateEnabledStatus();
        }

        private void UpdateEnabledStatus()
        {
            Enabled = ShouldBeEnabled();
        }

        private bool ShouldBeEnabled()
        {
            return _difficultyDropdown.Difficulty != _settingsManager.AIDifficulty;
        }
    }
}