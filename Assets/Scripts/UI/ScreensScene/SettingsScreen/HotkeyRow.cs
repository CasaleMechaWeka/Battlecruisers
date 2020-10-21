using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeyRow : MonoBehaviour, IPointerClickHandler
    {
        public HotkeyValueFeedback feedback;
        public HotkeyValue value;

        // FELIX  Implement, for SaveButton
        public IBroadcastingProperty<bool> HasChanged { get; private set; }

        public void Initialise(IInput input, KeyCode initialValue)
        {
            Helper.AssertIsNotNull(feedback, value);
            Assert.IsNotNull(input);

            feedback.Initialise(initialValue.ToString());
            value.Initialise(input, initialValue);
            value.Key.ValueChanged += Key_ValueChanged;
        }

        private void Key_ValueChanged(object sender, EventArgs e)
        {
            feedback.Value = value.Key.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // FELIX  NEXT
            // 1.5  Need to deselect other hotkey

            bool toggleValue = !value.enabled;
            value.enabled = toggleValue;
            feedback.IsSelected = toggleValue;
        }
    }
}