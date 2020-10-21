using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeyValueFeedback : MonoBehaviour
    {
        public Text text;
        public Image background;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                background.color = _isSelected ? Color.gray : Color.clear;
            }
        }

        public string Value
        {
            set
            {
                text.text = value;
            }
        }

        public void Initialise(string initialValue)
        {
            Helper.AssertIsNotNull(text, background);
            text.text = initialValue;
            IsSelected = false;
        }
    }
}