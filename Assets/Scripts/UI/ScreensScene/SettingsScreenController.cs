using System;
using System.Collections.Generic;
using BattleCruisers.Data.Settings;
using BattleCruisers.Scenes;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class SettingsScreenController : ScreenController
    {
        private ISettingsManager _settingsManager;
        private IList<Difficulty> _difficulties;
        private Dropdown _difficultyDropdown;
        private Slider _zoomSpeedSlider, _scrollSpeedSlider;

		public void Initialise(IScreensSceneGod screensSceneGod, ISettingsManager settingsManager)
		{
			base.Initialise(screensSceneGod);

            Assert.IsNotNull(settingsManager);
            _settingsManager = settingsManager;

            _difficultyDropdown = GetComponentInChildren<Dropdown>(includeInactive: true);
            Assert.IsNotNull(_difficultyDropdown);
			SetupDifficultyDropdown();

            _zoomSpeedSlider = transform.FindNamedComponent<Slider>("ZoomSpeedRow/Slider");
			SetupZoomSpeedSlider();
            
			_scrollSpeedSlider = transform.FindNamedComponent<Slider>("ScrollSpeedRow/Slider");
			SetupScrollSpeedSlider();
		}

        private void SetupDifficultyDropdown()
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            _difficulties = new List<Difficulty>();
            int currentIndex = 0;

            Difficulty[] difficulties = (Difficulty[])Enum.GetValues(typeof(Difficulty));

			for (int i = 0; i < difficulties.Length; ++i)
            {
                Difficulty difficulty = difficulties[i];

                options.Add(new Dropdown.OptionData(difficulty.ToString()));
                _difficulties.Add(difficulty);

                if (difficulty == _settingsManager.AIDifficulty)
                {
                    currentIndex = i;
                }
            }

            _difficultyDropdown.AddOptions(options);
            _difficultyDropdown.value = currentIndex;
        }

        private void SetupZoomSpeedSlider()
        {
            _zoomSpeedSlider.minValue = SettingsManager.MIN_ZOOM_SPEED;
            _zoomSpeedSlider.maxValue = SettingsManager.MAX_ZOOM_SPEED;
			_zoomSpeedSlider.value = _settingsManager.ZoomSpeed;
        }

		private void SetupScrollSpeedSlider()
		{
			_scrollSpeedSlider.minValue = SettingsManager.MIN_SCROLL_SPEED;
			_scrollSpeedSlider.maxValue = SettingsManager.MAX_SCROLL_SPEED;
			_scrollSpeedSlider.value = _settingsManager.ScrollSpeed;
		}

        public void Save()
        {
            Assert.IsTrue(_difficultyDropdown.value < _difficulties.Count);
            _settingsManager.AIDifficulty = _difficulties[_difficultyDropdown.value];

            _settingsManager.ZoomSpeed = _zoomSpeedSlider.value;
            _settingsManager.ScrollSpeed = _scrollSpeedSlider.value;

            _settingsManager.Save();

            _screensSceneGod.GoToHomeScreen();
        }

        public void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }
	}
}
