using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class ShipButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _attackBoatButton, _frigateButton, _destroyerButton, _archonButton, _attackRIBButton;

        public ShipButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton attackBoatButton,
            IBuildableButton frigateButton,
            IBuildableButton destroyerButton,
            IBuildableButton archonButton,
            IBuildableButton attackRIBButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(attackBoatButton, frigateButton, destroyerButton, archonButton);

            _attackBoatButton = attackBoatButton;
            _frigateButton = frigateButton;
            _destroyerButton = destroyerButton;
            _archonButton = archonButton;
            _attackRIBButton = attackRIBButton;

            _hotkeyDetector.AttackBoat += _hotkeyDetector_AttackBoat;
            _hotkeyDetector.Frigate += _hotkeyDetector_Frigate;
            _hotkeyDetector.Destroyer += _hotkeyDetector_Destroyer;
            _hotkeyDetector.Archon += _hotkeyDetector_Archon;
            _hotkeyDetector.AttackRIB += _hotkeyDetector_AttackRIB;
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

        private void _hotkeyDetector_AttackRIB(object sender, EventArgs e)
        {
            ClickIfPresented(_attackRIBButton);
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.AttackBoat -= _hotkeyDetector_AttackBoat;
            _hotkeyDetector.Frigate -= _hotkeyDetector_Frigate;
            _hotkeyDetector.Destroyer -= _hotkeyDetector_Destroyer;
            _hotkeyDetector.Archon -= _hotkeyDetector_Archon;
            _hotkeyDetector.AttackRIB -= _hotkeyDetector_AttackRIB;
        }
    }
}