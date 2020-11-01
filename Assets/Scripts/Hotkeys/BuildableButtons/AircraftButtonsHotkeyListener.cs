using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class AircraftButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _bomberButton, _gunshipButton, _fighterButton;

        public AircraftButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton bomberButton,
            IBuildableButton gunhsipButton,
            IBuildableButton fighterButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(bomberButton, gunhsipButton, fighterButton);

            _bomberButton = bomberButton;
            _gunshipButton = gunhsipButton;
            _fighterButton = fighterButton;

            _hotkeyDetector.Bomber += _hotkeyDetector_Bomber;
            _hotkeyDetector.Gunship += _hotkeyDetector_Gunship;
            _hotkeyDetector.Fighter += _hotkeyDetector_Fighter;
        }

        private void _hotkeyDetector_Bomber(object sender, EventArgs e)
        {
            ClickIfPresented(_bomberButton);
        }

        private void _hotkeyDetector_Gunship(object sender, EventArgs e)
        {
            ClickIfPresented(_gunshipButton);
        }

        private void _hotkeyDetector_Fighter(object sender, EventArgs e)
        {
            ClickIfPresented(_fighterButton);
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.Bomber -= _hotkeyDetector_Bomber;
            _hotkeyDetector.Gunship -= _hotkeyDetector_Gunship;
            _hotkeyDetector.Fighter -= _hotkeyDetector_Fighter;
        }
    }
}