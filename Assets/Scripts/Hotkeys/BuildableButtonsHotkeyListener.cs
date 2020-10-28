using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using System;

// FELIX  Test
namespace BattleCruisers.Hotkeys
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

            // FELIX
        }

        public void DisposeManagedState()
        {
        }
    }
}