using BattleCruisers.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons
{
    public class PvPAircraftButtonsHotkeyListener : PvPBuildableButtonHotkeyListener, IPvPManagedDisposable
    {
        private readonly IPvPBuildableButton _bomberButton, _gunshipButton, _fighterButton, _steamCopterButton, _broadswordButton;

        public PvPAircraftButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IPvPBuildableButton bomberButton,
            IPvPBuildableButton gunhsipButton,
            IPvPBuildableButton fighterButton,
            IPvPBuildableButton steamCopterButton)
            : base(hotkeyDetector)
        {
            PvPHelper.AssertIsNotNull(bomberButton, gunhsipButton, fighterButton);

            _bomberButton = bomberButton;
            _gunshipButton = gunhsipButton;
            _fighterButton = fighterButton;
            _steamCopterButton = steamCopterButton;

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
            _hotkeyDetector.AircraftButton1 += _hotkeyDetector_Bomber;
            _hotkeyDetector.AircraftButton2 += _hotkeyDetector_Gunship;
            _hotkeyDetector.AircraftButton3 += _hotkeyDetector_Fighter;
            _hotkeyDetector.AircraftButton4 += _hotkeyDetector_SteamCopter;
            _hotkeyDetector.AircraftButton5 += _hotkeyDetector_Broadsword;
        }
    }
}