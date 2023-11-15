using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.ScreensScene.SettingsScreen
{
    public class PvPToggleController : MonoBehaviour
    {
        private IPvPSettableBroadcastingProperty<bool> _isChecked;
        public IPvPBroadcastingProperty<bool> IsChecked { get; private set; }
        private Toggle toggle;

        public void Initialise(bool isChecked)
        {
            _isChecked = new PvPSettableBroadcastingProperty<bool>(isChecked);
            IsChecked = new PvPBroadcastingProperty<bool>(_isChecked);

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