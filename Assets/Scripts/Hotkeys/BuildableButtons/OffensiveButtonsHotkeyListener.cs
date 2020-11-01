using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys.BuildableButtons
{
    public class OffensiveButtonsHotkeyListener : BuildableButtonHotkeyListener, IManagedDisposable
    {
        private readonly IBuildableButton _artilleryButton, _railgunButton, _rocketLauncherButton;

        public OffensiveButtonsHotkeyListener(
            IHotkeyDetector hotkeyDetector,
            IBuildableButton artilleryButton,
            IBuildableButton railgunButton,
            IBuildableButton rocketLauncherButton)
            : base(hotkeyDetector)
        {
            Helper.AssertIsNotNull(artilleryButton, railgunButton, rocketLauncherButton);

            _artilleryButton = artilleryButton;
            _railgunButton = railgunButton;
            _rocketLauncherButton = rocketLauncherButton;

            _hotkeyDetector.Artillery += _hotkeyDetector_Artillery;
            _hotkeyDetector.Railgun += _hotkeyDetector_Railgun;
            _hotkeyDetector.RocketLauncher += _hotkeyDetector_RocketLauncher;
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

        public void DisposeManagedState()
        {
            _hotkeyDetector.Artillery -= _hotkeyDetector_Artillery;
            _hotkeyDetector.Railgun -= _hotkeyDetector_Railgun;
            _hotkeyDetector.RocketLauncher -= _hotkeyDetector_RocketLauncher;
        }
    }
}