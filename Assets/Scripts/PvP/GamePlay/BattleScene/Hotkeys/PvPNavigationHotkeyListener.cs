using BattleCruisers.Hotkeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Hotkeys
{
    public class PvPNavigationHotkeyListener : IPvPManagedDisposable
    {
        private readonly IHotkeyDetector _hotkeyDetector;
        private readonly IPvPCameraFocuser _cameraFocuser;

        public PvPNavigationHotkeyListener(IHotkeyDetector hotkeyDetector, IPvPCameraFocuser cameraFocuser)
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
                _cameraFocuser.FocusOnLeftPlayerCruiser();
            else
                _cameraFocuser.FocusOnRightPlayerCruiser();
        }

        private void _hotkeyDetector_Overview(object sender, EventArgs e)
        {
            _cameraFocuser.FocusOnOverview();
        }

        private void _hotkeyDetector_EnemyCruiser(object sender, EventArgs e)
        {
            if (SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT)
                _cameraFocuser.FocusOnRightPlayerCruiser();
            else
                _cameraFocuser.FocusOnLeftPlayerCruiser();
        }

        public void DisposeManagedState()
        {
            _hotkeyDetector.PlayerCruiser -= _hotkeyDetector_PlayerCruiser;
            _hotkeyDetector.Overview -= _hotkeyDetector_Overview;
            _hotkeyDetector.EnemyCruiser -= _hotkeyDetector_EnemyCruiser;
        }
    }
}