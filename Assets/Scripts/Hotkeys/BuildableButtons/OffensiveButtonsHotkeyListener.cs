using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class OffensiveButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _artilleryButton, _railgunButton, _rocketLauncherButton, _MLRSButton, _gatlingMortarButton;

        public OffensiveButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton artilleryButton,
            IBuildableButton railgunButton,
            IBuildableButton rocketLauncherButton,
            IBuildableButton MLRSButton,
            IBuildableButton gatlingMortarButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(artilleryButton, railgunButton, rocketLauncherButton);

            _artilleryButton = artilleryButton;
            _railgunButton = railgunButton;
            _rocketLauncherButton = rocketLauncherButton;
            _MLRSButton = MLRSButton;
            _gatlingMortarButton = gatlingMortarButton;

            _hotkeyDetector.Artillery += _hotkeyDetector_Artillery;
            _hotkeyDetector.Railgun += _hotkeyDetector_Railgun;
            _hotkeyDetector.RocketLauncher += _hotkeyDetector_RocketLauncher;
            _hotkeyDetector.MLRS += _hotkeyDetector_MLRS;
            _hotkeyDetector.GatlingMortar += _hotkeyDetector_GatlingMortar;
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
            _hotkeyDetector.Artillery -= _hotkeyDetector_Artillery;
            _hotkeyDetector.Railgun -= _hotkeyDetector_Railgun;
            _hotkeyDetector.RocketLauncher -= _hotkeyDetector_RocketLauncher;
            _hotkeyDetector.MLRS -= _hotkeyDetector_MLRS;
            _hotkeyDetector.GatlingMortar -= _hotkeyDetector_GatlingMortar;
        }
    }
}