using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class AircraftButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _bomberButton, _gunshipButton, _fighterButton, _steamCopterButton, _broadswordButton;

        public AircraftButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton bomberButton,
            IBuildableButton gunhsipButton,
            IBuildableButton fighterButton,
            IBuildableButton steamCopterButton,
            IBuildableButton broadswordButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(bomberButton, gunhsipButton, fighterButton);

            _bomberButton = bomberButton;
            _gunshipButton = gunhsipButton;
            _fighterButton = fighterButton;
            _steamCopterButton = steamCopterButton;
            _broadswordButton = broadswordButton;   

            _hotkeyDetector.AircraftButton1 += _hotkeyDetector_Bomber;
            _hotkeyDetector.AircraftButton2 += _hotkeyDetector_Gunship;
            _hotkeyDetector.AircraftButton3 += _hotkeyDetector_Fighter;
            _hotkeyDetector.AircraftButton4 += _hotkeyDetector_SteamCopter;
            _hotkeyDetector.AircraftButton5 += _hotkeyDetector_Broadsword;
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

        private void _hotkeyDetector_SteamCopter(object sender, EventArgs e)
        {
            ClickIfPresented(_steamCopterButton);
        }

        private void _hotkeyDetector_Broadsword(object sender, EventArgs e)
        {
            ClickIfPresented(_broadswordButton);
        }
        public void DisposeManagedState()
        {
            _hotkeyDetector.AircraftButton1 -= _hotkeyDetector_Bomber;
            _hotkeyDetector.AircraftButton2 -= _hotkeyDetector_Gunship;
            _hotkeyDetector.AircraftButton3 -= _hotkeyDetector_Fighter;
            _hotkeyDetector.AircraftButton4 -= _hotkeyDetector_SteamCopter;
            _hotkeyDetector.AircraftButton5 -= _hotkeyDetector_Broadsword;
        }
    }
}