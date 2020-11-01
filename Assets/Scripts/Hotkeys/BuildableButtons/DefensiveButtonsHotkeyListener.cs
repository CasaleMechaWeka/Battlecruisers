using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class DefensiveButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _shipTurretButton, _airTurretButton, _mortarButton, _samSiteButton, _teslaCoilButton;

        public DefensiveButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton shipTurretButton,
            IBuildableButton airTurretButton,
            IBuildableButton mortarButton,
            IBuildableButton samSiteButton,
            IBuildableButton teslaCoilButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(shipTurretButton, airTurretButton, mortarButton, samSiteButton, teslaCoilButton);

            _shipTurretButton = shipTurretButton;
            _airTurretButton = airTurretButton;
            _mortarButton = mortarButton;
            _samSiteButton = samSiteButton;
            _teslaCoilButton = teslaCoilButton;

            _hotkeyDetector.ShipTurret += _hotkeyDetector_ShipTurret;
            _hotkeyDetector.AirTurret += _hotkeyDetector_AirTurret;
            _hotkeyDetector.Mortar += _hotkeyDetector_Mortar;
            _hotkeyDetector.SamSite += _hotkeyDetector_SamSite;
            _hotkeyDetector.TeslaCoil += _hotkeyDetector_TeslaCoil;
        }

        private void _hotkeyDetector_ShipTurret(object sender, EventArgs e)
        {
            ClickIfPresented(_shipTurretButton);
        }

        private void _hotkeyDetector_AirTurret(object sender, EventArgs e)
        {
            ClickIfPresented(_airTurretButton);
        }

        private void _hotkeyDetector_Mortar(object sender, EventArgs e)
        {
            ClickIfPresented(_mortarButton);
        }

        private void _hotkeyDetector_SamSite(object sender, EventArgs e)
        {
            ClickIfPresented(_samSiteButton);
        }

        private void _hotkeyDetector_TeslaCoil(object sender, EventArgs e)
        {
            ClickIfPresented(_teslaCoilButton);
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.ShipTurret -= _hotkeyDetector_ShipTurret;
            _hotkeyDetector.AirTurret -= _hotkeyDetector_AirTurret;
            _hotkeyDetector.Mortar -= _hotkeyDetector_Mortar;
            _hotkeyDetector.SamSite -= _hotkeyDetector_SamSite;
            _hotkeyDetector.TeslaCoil -= _hotkeyDetector_TeslaCoil;
        }
    }
}