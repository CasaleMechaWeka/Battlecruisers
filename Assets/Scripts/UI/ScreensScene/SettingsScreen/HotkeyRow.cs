using BattleCruisers.UI.Panels;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeyRow : MonoBehaviour
    {
        public InputField input;

        // FELIX  For SaveButton
        public IBroadcastingProperty<bool> HasChanged { get; private set; }

        public void Initialise()
        {
            Assert.IsNotNull(input);

            //  FELIX  TEMP
            //input.onValueChanged.AddListener(OnValueChanged);

            // FELIX  TEMP
            input.text = KeyCode.DownArrow.ToString();
        }

        private void OnValueChanged(string inputString)
        {
            Debug.Log($"{inputString}  input.text: {input.text}");

            // Restrict input to a single character
            if (input.text.Length > 1)
            {
                input.text = input.text[input.text.Length - 1].ToString();
            }

            // FELIX  Will have to work with keycodes, not strings, so that non text keys (eg: arrows) can be used for hotkeys
            KeyCode t;
        }
    }
}