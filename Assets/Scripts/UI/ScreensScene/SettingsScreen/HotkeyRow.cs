using BattleCruisers.UI.Panels;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeyRow : MonoBehaviour
    {
        public InputField input;

        public void Initialise()
        {
            Assert.IsNotNull(input);

            input.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string inputString)
        {
            Debug.Log($"{inputString}  input.text: {input.text}");

            // Restrict input to a single character
            if (input.text.Length > 1)
            {
                input.text = input.text[input.text.Length - 1].ToString();
            }
        }
    }
}