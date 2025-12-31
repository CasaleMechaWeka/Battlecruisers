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

        public void Initialise(Vector2 selectedResolution)
        {
            _resolutionDropdown = GetComponent<Dropdown>();
            Assert.IsNotNull(_resolutionDropdown);

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            int currentIndex = -1;

            for (int i = 0; i < resolutions.Count; ++i)
                options.Add(new Dropdown.OptionData(resolutions[i].x + "x" + resolutions[i].y));

            Vector2 nativeResolution = new Vector2(Display.main.systemWidth, Display.main.systemHeight);

            if (!resolutions.Contains(nativeResolution))
                for (int i = 0; i < resolutions.Count; i++)
                    if (i == resolutions.Count
                    || nativeResolution.x > resolutions[i].x
                    || nativeResolution.x == resolutions[i].x && nativeResolution.y >= resolutions[i].y)
                    {
                        resolutions.Insert(i, nativeResolution);
                        options.Insert(i, new Dropdown.OptionData($"{nativeResolution.x} x {nativeResolution.y} (native)"));
                        break;
                    }

            if (currentIndex == -1)
                if (!resolutions.Contains(selectedResolution))
                    for (int i = 0; i < resolutions.Count; i++)
                        if (i == resolutions.Count
                        || selectedResolution.x > resolutions[i].x
                        || selectedResolution.x == resolutions[i].x && selectedResolution.y >= resolutions[i].y)
                        {
                            resolutions.Insert(i, selectedResolution);
                            options.Insert(i, new Dropdown.OptionData($"{selectedResolution.x} x {selectedResolution.y}"));
                            currentIndex = i;
                            break;
                        }

            if (currentIndex == -1)
                for (int i = 0; i < resolutions.Count; ++i)
                    if (resolutions[i].Equals(selectedResolution))
                        currentIndex = i;

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