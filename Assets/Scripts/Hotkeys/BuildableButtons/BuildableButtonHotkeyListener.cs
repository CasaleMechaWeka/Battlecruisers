using BattleCruisers.UI.BattleScene.Buttons;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public abstract class BuildableButtonHotkeyListener
    {
        protected readonly IHotkeyDetector _hotkeyDetector;

        protected BuildableButtonHotkeyListener(IHotkeyDetector hotkeyDetector)
        {
            Assert.IsNotNull(hotkeyDetector);
            _hotkeyDetector = hotkeyDetector;
        }

        protected void ClickIfPresented(IBuildableButton button)
        {
            if (button.IsPresented)
            {
                button.TriggerClick();
            }
        }
    }
}