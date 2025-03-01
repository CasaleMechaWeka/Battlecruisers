using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Hotkeys
{
    public class NavigationHotkeyListener : IManagedDisposable
    {
        private readonly IHotkeyDetector _hotkeyDetector;
        private readonly ICameraFocuser _cameraFocuser;

        public NavigationHotkeyListener(IHotkeyDetector hotkeyDetector, ICameraFocuser cameraFocuser)
        {
            Helper.AssertIsNotNull(hotkeyDetector, cameraFocuser);

            _hotkeyDetector = hotkeyDetector;
            _cameraFocuser = cameraFocuser;

            _hotkeyDetector.PlayerCruiser += _hotkeyDetector_PlayerCruiser;
            _hotkeyDetector.Overview += _hotkeyDetector_Overview;
            _hotkeyDetector.EnemyCruiser += _hotkeyDetector_EnemyCruiser;
        }

        private void _hotkeyDetector_PlayerCruiser(object sender, EventArgs e)
        {
            _cameraFocuser.FocusOnLeftCruiser();
        }

        private void _hotkeyDetector_Overview(object sender, EventArgs e)
        {
            _cameraFocuser.FocusOnOverview();
        }

        private void _hotkeyDetector_EnemyCruiser(object sender, EventArgs e)
        {
            _cameraFocuser.FocusOnRightCruiser();
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.PlayerCruiser -= _hotkeyDetector_PlayerCruiser;
            _hotkeyDetector.Overview -= _hotkeyDetector_Overview;
            _hotkeyDetector.EnemyCruiser -= _hotkeyDetector_EnemyCruiser;
        }
    }
}