using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons
{
    public class PvPDefensiveButtonsHotkeyListener : PvPBuildableButtonHotkeyListener, IPvPManagedDisposable
    {
        private readonly IPvPBuildableButton _shipTurretButton, _airTurretButton, _mortarButton, _samSiteButton, _teslaCoilButton;

        public PvPDefensiveButtonsHotkeyListener(
            IPvPHotkeyDetector hotkeyDetector,
            IPvPBuildableButton shipTurretButton,
            IPvPBuildableButton airTurretButton,
            IPvPBuildableButton mortarButton,
            IPvPBuildableButton samSiteButton,
            IPvPBuildableButton teslaCoilButton)
            : base(hotkeyDetector)
        {
            PvPHelper.AssertIsNotNull(shipTurretButton, airTurretButton, mortarButton, samSiteButton, teslaCoilButton);

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