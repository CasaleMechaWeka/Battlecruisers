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

        public List<Sprite> flagSymbols;
        public List<string> languageCodeNames;
        public List<string> languageNativeNames;
        public string Language => languageCodeNames[_languageDropdown.value];

        public event EventHandler LanguageChanged;

        public void Initialise(string selectedLanguage, ILocTable loc)
        {
            Assert.IsNotNull(loc);

            _languageDropdown = GetComponent<Dropdown>();
            Assert.IsNotNull(_languageDropdown);

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            int currentIndex = 0;
            //Assert.AreEqual(flagSymbols.Count, difficultiesNoEasy.Count);

            for (int i = 0; i < flagSymbols.Count; ++i)
            {
                options.Add(new Dropdown.OptionData(languageNativeNames[i], flagSymbols[i]));
                if (languageCodeNames[i] == selectedLanguage)
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
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ResetToDefaults(string defaultLanguage)
        {
            int defaultIndex = languageCodeNames.IndexOf(defaultLanguage);
            _languageDropdown.value = defaultIndex;
        }
    }
}