using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class ToggleController : MonoBehaviour
    {
        private ISettableBroadcastingProperty<bool> _isChecked;
        public IBroadcastingProperty<bool> IsChecked { get; private set; }

        public void Initialise(bool isChecked)
        {
            Toggle toggle = GetComponentInChildren<Toggle>();
            Assert.IsNotNull(toggle);
            toggle.onValueChanged.AddListener(OnValueChanged);

            _isChecked = new SettableBroadcastingProperty<bool>(isChecked);
            IsChecked = new BroadcastingProperty<bool>(_isChecked);
        }

        private void OnValueChanged(bool value)
        {
            _isChecked.Value = value;
        }
    }
}