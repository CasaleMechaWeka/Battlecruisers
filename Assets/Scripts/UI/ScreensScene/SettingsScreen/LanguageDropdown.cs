using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class LanguageDropdown : MonoBehaviour
    {
        private Dropdown _languageDropdown;
        private IList<string> _languages;

        public List<Sprite> flagSymbols;
        public List<string> languages;
        public string Language => _languages[_languageDropdown.value];

        public event EventHandler LanguageChanged;

        public void Initialise(string selectedLanguage, ILocTable loc)
        {
            Assert.IsNotNull(loc);

            _languageDropdown = GetComponent<Dropdown>();
            Assert.IsNotNull(_languageDropdown);

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            _languages = new List<string>();
            int currentIndex = 0;
            //Assert.AreEqual(flagSymbols.Count, difficultiesNoEasy.Count);

            for (int i = 0; i < flagSymbols.Count; ++i)
            {
                

                options.Add(new Dropdown.OptionData(languages[i], flagSymbols[i]));
                _languages.Add(languages[i]);

                if (languages[i] == selectedLanguage)
                {
                    currentIndex = i;
                }
            }

            _languageDropdown.AddOptions(options);
            _languageDropdown.value = currentIndex;
            _languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        }

        private void OnLanguageChanged(int dropdownIndex)
        {
            Assert.IsTrue(_languageDropdown.value < _languages.Count);

            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ResetToDefaults(string defaultLanguage)
        {
            int defaultIndex = _languages.IndexOf(defaultLanguage);
            _languageDropdown.value = defaultIndex;
        }
    }
}