using BattleCruisers.Data.Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class DifficultyDropdown : MonoBehaviour, IDifficultyDropdown
    {
        private Dropdown _difficultyDropdown;
        private IList<Difficulty> _difficulties;

        public Difficulty Difficulty { get { return _difficulties[_difficultyDropdown.value]; } }

        public event EventHandler DifficultyChanged;

        public void Initialise(Difficulty selectedDifficulty)
        {
            _difficultyDropdown = GetComponent<Dropdown>();
            Assert.IsNotNull(_difficultyDropdown);

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            _difficulties = new List<Difficulty>();
            int currentIndex = 0;

            Difficulty[] difficulties = (Difficulty[])Enum.GetValues(typeof(Difficulty));

            for (int i = 0; i < difficulties.Length; ++i)
            {
                Difficulty difficulty = difficulties[i];

                options.Add(new Dropdown.OptionData(difficulty.ToString()));
                _difficulties.Add(difficulty);

                if (difficulty == selectedDifficulty)
                {
                    currentIndex = i;
                }
            }

            _difficultyDropdown.AddOptions(options);
            _difficultyDropdown.value = currentIndex;
            _difficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);
        }

        private void OnDifficultyChanged(int dropdownIndex)
        {
            Assert.IsTrue(_difficultyDropdown.value < _difficulties.Count);

            DifficultyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}