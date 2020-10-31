using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

// FELIX  Test
namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class ShipButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _attackBoatButton, _frigateButton, _destroyerButton, _archonButton;

        public ShipButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton attackBoatButton,
            IBuildableButton frigateButton,
            IBuildableButton destroyerButton,
            IBuildableButton archonButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(attackBoatButton, frigateButton, destroyerButton, archonButton);

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
        public void DisposeManagedState()
        {
            _hotkeyDetector.AttackBoat -= _hotkeyDetector_AttackBoat;
            _hotkeyDetector.Frigate -= _hotkeyDetector_Frigate;
            _hotkeyDetector.Destroyer -= _hotkeyDetector_Destroyer;
            _hotkeyDetector.Archon -= _hotkeyDetector_Archon;
        }
    }
}