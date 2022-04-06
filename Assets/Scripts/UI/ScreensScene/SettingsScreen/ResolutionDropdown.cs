using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class ResolutionDropdown : MonoBehaviour
    {
        private Dropdown _resolutionDropdown;
        public List<Vector2> resolutions;
        public Vector2 Resolution => resolutions[_resolutionDropdown.value];

        public event EventHandler ResolutionChanged;

        public void Initialise(Vector2 selectedResolution, ILocTable loc)
        {
            Assert.IsNotNull(loc);

            _resolutionDropdown = GetComponent<Dropdown>();
            Assert.IsNotNull(_resolutionDropdown);

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            int currentIndex = -1;

            for (int i = 0; i < resolutions.Count; ++i)
            {
                options.Add(new Dropdown.OptionData(resolutions[i].x + "x" + resolutions[i].y));
                if (resolutions[i].Equals(selectedResolution))
                {
                    currentIndex = i;
                }
            }
            if (currentIndex == -1)
            {
                options.Add(new Dropdown.OptionData(selectedResolution.x + "x" + selectedResolution.y));
                resolutions.Add(selectedResolution);
                currentIndex = resolutions.Count -1;
            }

            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = currentIndex;
            _resolutionDropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(int dropdownIndex)
        {
            ResolutionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ResetToDefaults(Vector2 defaultResolution)
        {
            int defaultIndex = resolutions.IndexOf(defaultResolution);
            _resolutionDropdown.value = defaultIndex;
        }
    }
}