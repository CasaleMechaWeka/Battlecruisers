using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class AircraftButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _bomberButton, _gunshipButton, _fighterButton, _steamCopterButton;

        public AircraftButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton bomberButton,
            IBuildableButton gunhsipButton,
            IBuildableButton fighterButton,
            IBuildableButton steamCopterButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(bomberButton, gunhsipButton, fighterButton);

            _bomberButton = bomberButton;
            _gunshipButton = gunhsipButton;
            _fighterButton = fighterButton;
            _steamCopterButton = steamCopterButton;

            _hotkeyDetector.Bomber += _hotkeyDetector_Bomber;
            _hotkeyDetector.Gunship += _hotkeyDetector_Gunship;
            _hotkeyDetector.Fighter += _hotkeyDetector_Fighter;
            _hotkeyDetector.SteamCopter += _hotkeyDetector_SteamCopter;
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

        public void DisposeManagedState()
        {
            _hotkeyDetector.Bomber -= _hotkeyDetector_Bomber;
            _hotkeyDetector.Gunship -= _hotkeyDetector_Gunship;
            _hotkeyDetector.Fighter -= _hotkeyDetector_Fighter;
            _hotkeyDetector.SteamCopter -= _hotkeyDetector_SteamCopter;
        }
    }
}