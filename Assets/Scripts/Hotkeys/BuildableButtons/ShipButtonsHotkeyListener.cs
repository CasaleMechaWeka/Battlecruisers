using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

// FELIX  Test
// FELIX  Create class for single hotkey to avoid duplicate code?  Takes event and button?
namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class ShipButtonsHotkeyListener : IManagedDisposable
    {
        private readonly IHotkeyDetector _hotkeyDetector;
        private readonly IBuildableButton _attackBoatButton, _frigateButton, _destroyerButton, _archonButton;

        public ShipButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton attackBoatButton,
            IBuildableButton frigateButton,
            IBuildableButton destroyerButton,
            IBuildableButton archonButton)
        {
            Helper.AssertIsNotNull(hotkeyDetector, attackBoatButton, frigateButton, destroyerButton, archonButton);

            _hotkeyDetector = hotkeyDetector;

            _attackBoatButton = attackBoatButton;
            _frigateButton = frigateButton;
            _destroyerButton = destroyerButton;
            _archonButton = archonButton;

            _hotkeyDetector.AttackBoat += _hotkeyDetector_AttackBoat;
            _hotkeyDetector.Frigate += _hotkeyDetector_Frigate;
            _hotkeyDetector.Destroyer += _hotkeyDetector_Destroyer;
            _hotkeyDetector.Archon += _hotkeyDetector_Archon;
        }

        private void _hotkeyDetector_AttackBoat(object sender, EventArgs e)
        {
            ClickIfPresented(_attackBoatButton);
        }

        private void _hotkeyDetector_Frigate(object sender, EventArgs e)
        {
            ClickIfPresented(_frigateButton);
        }

        private void _hotkeyDetector_Destroyer(object sender, EventArgs e)
        {
            ClickIfPresented(_destroyerButton);
        }

        private void _hotkeyDetector_Archon(object sender, EventArgs e)
        {
            ClickIfPresented(_archonButton);
        }

        private void ClickIfPresented(IBuildableButton button)
        {
            if (button.IsPresented)
            {
                button.TriggerClick();
            }
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.AttackBoat -= _hotkeyDetector_AttackBoat;
            _hotkeyDetector.Frigate -= _hotkeyDetector_Frigate;
            _hotkeyDetector.Destroyer -= _hotkeyDetector_Destroyer;
            _hotkeyDetector.Archon -= _hotkeyDetector_Archon;
        }
    }
}