using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class StringDropdown : MonoBehaviour, IStringDropdown
    {
        private Dropdown _dropdown;
        private IList<string> _values;

        // FELIX  Remove?  Use index instead?
        public string SelectedValue => _values[_dropdown.value];
        public int SelectedIndex => _dropdown.value;

        public void Initialise(IList<string> values, string initialValue)
        {
            Assert.IsNotNull(values);
            Assert.IsFalse(string.IsNullOrEmpty(initialValue));

            _dropdown = GetComponent<Dropdown>();
            Assert.IsNotNull(_dropdown);

            _values = values;

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            int currentIndex = 0;

            for (int i = 0; i < _values.Count; ++i)
            {
                string value = _values[i];

                options.Add(new Dropdown.OptionData(value));

                if (value == initialValue)
                {
                    currentIndex = i;
                }
            }

            _dropdown.AddOptions(options);
            _dropdown.value = currentIndex;
        }
    }
}