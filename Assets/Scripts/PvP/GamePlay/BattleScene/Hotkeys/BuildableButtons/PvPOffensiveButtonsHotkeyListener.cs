using BattleCruisers.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys.BuildableButtons
{
    public class PvPOffensiveButtonsHotkeyListener : PvPBuildableButtonHotkeyListener, IPvPManagedDisposable
    {
        private readonly IPvPBuildableButton _artilleryButton, _railgunButton, _rocketLauncherButton, _MLRSButton, _gatlingMortarButton;

        public PvPOffensiveButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IPvPBuildableButton artilleryButton,
            IPvPBuildableButton railgunButton,
            IPvPBuildableButton rocketLauncherButton,
            IPvPBuildableButton MLRSButton,
            IPvPBuildableButton gatlingMortarButton)
            : base(hotkeyDetector)
        {
            PvPHelper.AssertIsNotNull(artilleryButton, railgunButton, rocketLauncherButton);

            _artilleryButton = artilleryButton;
            _railgunButton = railgunButton;
            _rocketLauncherButton = rocketLauncherButton;
            _MLRSButton = MLRSButton;
            _gatlingMortarButton = gatlingMortarButton;

            _hotkeyDetector.OffensiveButton1 += _hotkeyDetector_Artillery;
            _hotkeyDetector.OffensiveButton2 += _hotkeyDetector_Railgun;
            _hotkeyDetector.OffensiveButton3 += _hotkeyDetector_RocketLauncher;
            _hotkeyDetector.OffensiveButton4 += _hotkeyDetector_MLRS;
            _hotkeyDetector.OffensiveButton5 += _hotkeyDetector_GatlingMortar;
        }

        private void _hotkeyDetector_Artillery(object sender, EventArgs e)
        {
            ClickIfPresented(_artilleryButton);
        }

        private void _hotkeyDetector_Railgun(object sender, EventArgs e)
        {
            ClickIfPresented(_railgunButton);
        }

        private void _hotkeyDetector_RocketLauncher(object sender, EventArgs e)
        {
            ClickIfPresented(_rocketLauncherButton);
        }

        private void _hotkeyDetector_MLRS(object sender, EventArgs e)
        {
            ClickIfPresented(_MLRSButton);
        }

        private void _hotkeyDetector_GatlingMortar(object sender, EventArgs e)
        {
            ClickIfPresented(_gatlingMortarButton);
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.OffensiveButton1 -= _hotkeyDetector_Artillery;
            _hotkeyDetector.OffensiveButton2 -= _hotkeyDetector_Railgun;
            _hotkeyDetector.OffensiveButton3 -= _hotkeyDetector_RocketLauncher;
            _hotkeyDetector.OffensiveButton4 -= _hotkeyDetector_MLRS;
            _hotkeyDetector.OffensiveButton5 -= _hotkeyDetector_GatlingMortar;
        }
    }
}