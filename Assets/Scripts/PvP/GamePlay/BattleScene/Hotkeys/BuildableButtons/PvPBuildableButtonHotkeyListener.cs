using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons
{
    public abstract class PvPBuildableButtonHotkeyListener
    {
        protected readonly IPvPHotkeyDetector _hotkeyDetector;

        protected PvPBuildableButtonHotkeyListener(IPvPHotkeyDetector hotkeyDetector)
        {
            Assert.IsNotNull(hotkeyDetector);
            _hotkeyDetector = hotkeyDetector;
        }

        protected void ClickIfPresented(IPvPBuildableButton button)
        {
            if (button.IsPresented)
            {
                button.TriggerClick();
            }
        }
    }
}