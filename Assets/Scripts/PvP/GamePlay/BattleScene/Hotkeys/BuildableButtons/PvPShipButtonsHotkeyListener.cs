using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons
{
    public class PvPShipButtonsHotkeyListener : PvPBuildableButtonHotkeyListener, IPvPManagedDisposable
    {
        private readonly IPvPBuildableButton _attackBoatButton, _frigateButton, _destroyerButton, _archonButton, _attackRIBButton;

        public PvPShipButtonsHotkeyListener(
            IPvPHotkeyDetector hotkeyDetector,
            IPvPBuildableButton attackBoatButton,
            IPvPBuildableButton frigateButton,
            IPvPBuildableButton destroyerButton,
            IPvPBuildableButton archonButton,
            IPvPBuildableButton attackRIBButton)
            : base(hotkeyDetector)
        {
            PvPHelper.AssertIsNotNull(attackBoatButton, frigateButton, destroyerButton, archonButton);

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