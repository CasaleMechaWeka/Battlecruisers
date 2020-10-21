using BattleCruisers.UI.Panels;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeyRow : MonoBehaviour, IPointerClickHandler
    {
        public InputField input;

        // FELIX  Implement, for SaveButton
        public IBroadcastingProperty<bool> HasChanged { get; private set; }

        public void Initialise()
        {
            Assert.IsNotNull(input);

            //  FELIX  TEMP
            //input.onValueChanged.AddListener(OnValueChanged);

            // FELIX  TEMP
            input.text = KeyCode.DownArrow.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // FELIX  NEXT
            // 1. Show feedback that this row is being edited
            // 2. Enable HotkeyValue script
            throw new System.NotImplementedException();
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