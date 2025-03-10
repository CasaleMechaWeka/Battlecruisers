using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class ToggleController : MonoBehaviour
    {
        private ISettableBroadcastingProperty<bool> _isChecked;
        public IBroadcastingProperty<bool> IsChecked { get; private set; }
        private Toggle toggle;

        public void Initialise(bool isChecked)
        {
            _isChecked = new SettableBroadcastingProperty<bool>(isChecked);
            IsChecked = new BroadcastingProperty<bool>(_isChecked);

            toggle = GetComponentInChildren<Toggle>();
            Assert.IsNotNull(toggle);
            toggle.onValueChanged.AddListener(OnValueChanged);
            toggle.isOn = isChecked;
        }

        private void OnValueChanged(bool value)
        {
            _isChecked.Value = value;
        }

        public void ResetToDefaults(bool isChecked)
        {
            _isChecked.Value = isChecked;
            toggle.isOn = isChecked;
        }
    }
}