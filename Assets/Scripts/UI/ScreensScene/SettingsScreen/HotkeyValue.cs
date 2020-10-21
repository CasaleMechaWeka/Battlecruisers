using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    /// <summary>
    /// Work is done on every update.  Hence this script should only be enabled
    /// when the user wants to change a particular hotkey.
    /// </summary>
    public class HotkeyValue : MonoBehaviour
    {
        private IInput _input;

        private ISettableBroadcastingProperty<KeyCode> _key;
        public IBroadcastingProperty<KeyCode> Key { get; private set; }

        public void Initialise(IInput input, KeyCode key)
        {
            Assert.IsNotNull(input);

            _input = input;

            _key = new SettableBroadcastingProperty<KeyCode>(key);
            Key = new BroadcastingProperty<KeyCode>(_key);
        }

        void Update()
        {
            KeyCode downedKey = _input.GetFirstKeyDown();

            if (downedKey != KeyCode.None)
            {
                _key.Value = downedKey;
            }
        }
    }
}