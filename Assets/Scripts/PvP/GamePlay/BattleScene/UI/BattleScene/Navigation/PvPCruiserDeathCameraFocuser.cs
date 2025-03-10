using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Offensive;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.Navigation;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPCruiserDeathCameraFocuser : IPvPCruiserDeathCameraFocuser
    {
        private readonly ICameraFocuser _cameraFocuser;

        public PvPCruiserDeathCameraFocuser(ICameraFocuser cameraFocuser)
        {
            Assert.IsNotNull(cameraFocuser);
            _cameraFocuser = cameraFocuser;
        }

        public void FocusOnLosingCruiser(IPvPCruiser losingCruiser)
        {
            if (losingCruiser.IsPlayerCruiser)
            {
                if (IsNukeCauseOfDeath(losingCruiser))
                {
                    _cameraFocuser.FocusOnLeftCruiserNuke();
                }
                else
                {
                    _cameraFocuser.FocusOnLeftCruiserDeath();
                }
            }
            else
            {
                if (IsNukeCauseOfDeath(losingCruiser))
                {
                    _cameraFocuser.FocusOnRightCruiserNuke();
                }
                else
                {
                    _cameraFocuser.FocusOnRightCruiserDeath();
                }
            }
        }

        public void FocusOnDisconnectedCruiser(bool isHost)
        {
            if (isHost)
            {
                _cameraFocuser.FocusOnRightCruiserDeath();
            }
            else
            {
                _cameraFocuser.FocusOnLeftCruiserDeath();
            }
        }

        private bool IsNukeCauseOfDeath(IPvPCruiser losingCruiser)
        {
            return losingCruiser.LastDamagedSource is PvPNukeLauncherController;
        }
    }
}