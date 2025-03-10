using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class TacticalButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _shieldButton, _boosterButton, _stealthGeneratorButton, _spySatelliteButton, _controlTowerButon;

        public TacticalButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton shieldButton,
            IBuildableButton boosterButton,
            IBuildableButton stealthGeneratorButton,
            IBuildableButton spySatelliteButton,
            IBuildableButton controlToworButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(shieldButton, boosterButton, stealthGeneratorButton, spySatelliteButton, controlToworButton);

            _shieldButton = shieldButton;
            _boosterButton = boosterButton;
            _stealthGeneratorButton = stealthGeneratorButton;
            _spySatelliteButton = spySatelliteButton;
            _controlTowerButon = controlToworButton;

            _hotkeyDetector.TacticalButton1 += _hotkeyDetector_Shield;
            _hotkeyDetector.TacticalButton2 += _hotkeyDetector_Booster;
            _hotkeyDetector.TacticalButton3 += _hotkeyDetector_StealthGenerator;
            _hotkeyDetector.TacticalButton4 += _hotkeyDetector_SpySatellite;
            _hotkeyDetector.TacticalButton5 += _hotkeyDetector_ControlTower;
        }

        private void _hotkeyDetector_Shield(object sender, EventArgs e)
        {
            ClickIfPresented(_shieldButton);
        }

        private void _hotkeyDetector_Booster(object sender, EventArgs e)
        {
            ClickIfPresented(_boosterButton);
        }

        private void _hotkeyDetector_StealthGenerator(object sender, EventArgs e)
        {
            ClickIfPresented(_stealthGeneratorButton);
        }

        private void _hotkeyDetector_SpySatellite(object sender, EventArgs e)
        {
            ClickIfPresented(_spySatelliteButton);
        }

        private void _hotkeyDetector_ControlTower(object sender, EventArgs e)
        {
            ClickIfPresented(_controlTowerButon);
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.TacticalButton1  -= _hotkeyDetector_Shield;
            _hotkeyDetector.TacticalButton2 -= _hotkeyDetector_Booster;
            _hotkeyDetector.TacticalButton3 -= _hotkeyDetector_StealthGenerator;
            _hotkeyDetector.TacticalButton4 -= _hotkeyDetector_SpySatellite;
            _hotkeyDetector.TacticalButton5 -= _hotkeyDetector_ControlTower;
        }
    }
}