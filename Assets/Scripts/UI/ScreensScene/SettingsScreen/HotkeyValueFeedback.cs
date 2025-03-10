using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeyValueFeedback : MonoBehaviour
    {
        public Text text;
        public Image selectedFeedback;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                selectedFeedback.enabled = value;
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
            Helper.AssertIsNotNull(text, selectedFeedback);
            text.text = initialValue;
            IsSelected = false;
        }
    }
}