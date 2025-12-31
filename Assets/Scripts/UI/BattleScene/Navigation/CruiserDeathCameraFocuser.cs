using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class CruiserDeathCameraFocuser
    {
        private readonly ICameraFocuser _cameraFocuser;

        public CruiserDeathCameraFocuser(ICameraFocuser cameraFocuser)
        {
            Assert.IsNotNull(cameraFocuser);
            _cameraFocuser = cameraFocuser;
        }

        public void FocusOnLosingCruiser(ICruiser losingCruiser)
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

        private bool IsNukeCauseOfDeath(ICruiser losingCruiser)
        {
            return losingCruiser.LastDamagedSource is NukeLauncherController;
        }
    }
}