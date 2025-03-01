using BattleCruisers.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys
{
    public class PvPNavigationHotkeyListener : IManagedDisposable
    {
        private readonly IHotkeyDetector _hotkeyDetector;
        private readonly ICameraFocuser _cameraFocuser;

        public PvPNavigationHotkeyListener(IHotkeyDetector hotkeyDetector, ICameraFocuser cameraFocuser)
        {
            PvPHelper.AssertIsNotNull(hotkeyDetector, cameraFocuser);

            _hotkeyDetector = hotkeyDetector;
            _cameraFocuser = cameraFocuser;

            _hotkeyDetector.PlayerCruiser += _hotkeyDetector_PlayerCruiser;
            _hotkeyDetector.Overview += _hotkeyDetector_Overview;
            _hotkeyDetector.EnemyCruiser += _hotkeyDetector_EnemyCruiser;
        }

        private void _hotkeyDetector_PlayerCruiser(object sender, EventArgs e)
        {
            if (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT)
                _cameraFocuser.FocusOnLeftCruiser();
            else
                _cameraFocuser.FocusOnRightCruiser();
        }

        private void _hotkeyDetector_Overview(object sender, EventArgs e)
        {
            _cameraFocuser.FocusOnOverview();
        }

        private void _hotkeyDetector_EnemyCruiser(object sender, EventArgs e)
        {
            if (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT)
                _cameraFocuser.FocusOnRightCruiser();
            else
                _cameraFocuser.FocusOnLeftCruiser();
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.PlayerCruiser -= _hotkeyDetector_PlayerCruiser;
            _hotkeyDetector.Overview -= _hotkeyDetector_Overview;
            _hotkeyDetector.EnemyCruiser -= _hotkeyDetector_EnemyCruiser;
        }
    }
}