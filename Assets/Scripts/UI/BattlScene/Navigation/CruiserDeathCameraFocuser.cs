using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Cruisers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    // FELIX  test
    public class CruiserDeathCameraFocuser : ICruiserDeathCameraFocuser
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

        private bool IsNukeCauseOfDeath(ICruiser losingCruiser)
        {
            return losingCruiser.LastDamagedSource is NukeLauncherController;
        }
    }
}