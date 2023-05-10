using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Offensive;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPCruiserDeathCameraFocuser : IPvPCruiserDeathCameraFocuser
    {
        private readonly IPvPCameraFocuser _cameraFocuser;

        public PvPCruiserDeathCameraFocuser(IPvPCameraFocuser cameraFocuser)
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
                    _cameraFocuser.FocusOnPlayerCruiserNuke();
                }
                else
                {
                    _cameraFocuser.FocusOnPlayerCruiserDeath();
                }
            }
            else
            {
                if (IsNukeCauseOfDeath(losingCruiser))
                {
                    _cameraFocuser.FocusOnAICruiserNuke();
                }
                else
                {
                    _cameraFocuser.FocusOnAICruiserDeath();
                }
            }
        }

        private bool IsNukeCauseOfDeath(IPvPCruiser losingCruiser)
        {
            return losingCruiser.LastDamagedSource is PvPNukeLauncherController;
        }
    }
}