using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class HotkeyRow : MonoBehaviour, IPointerClickHandler
    {
        public HotkeyValueFeedback feedback;

        public HotkeyValue value;
        public HotkeyValue Value => value;

        public event EventHandler Enabled;

        public void Initialise(IInput input, KeyCode initialValue, HotkeysPanel hotkeysPanel)
        {
            Helper.AssertIsNotNull(feedback, value, hotkeysPanel);
            Assert.IsNotNull(input);

            feedback.Initialise(initialValue.ToString());
            value.Initialise(input, initialValue);
            value.Key.ValueChanged += Key_ValueChanged;

            hotkeysPanel.RowEnabled += HotkeysPanel_RowEnabled;
        }

        private void Key_ValueChanged(object sender, EventArgs e)
        {
            feedback.Value = value.Key.Value.ToString();
        }

        private void HotkeysPanel_RowEnabled(object sender, HotkeyRowEnabledEventArgs e)
        {
            if (!ReferenceEquals(e.RowEnabled, this))
            {
                SetEnabled(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            bool toggleValue = !value.enabled;
            SetEnabled(toggleValue);

            if (toggleValue)
            {
                Enabled?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ResetToDefaults(KeyCode defaultValue)
        {
            SetEnabled(false);
            value.ResetToDefaults(defaultValue);
        }

        private void SetEnabled(bool isEnabled)
        {
            value.enabled = isEnabled;
            feedback.IsSelected = isEnabled;
        }
    }
}