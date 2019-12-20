using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    // FELIX  Update tests :)
    public class CameraFocuser : ICameraFocuser
    {
        private readonly INavigationWheelPositionProvider _positionProvider;
        private readonly INavigationWheel _navigationWheel;
        private readonly IStaticCameraTargetProvider _trumpCameraTargetProvider;

        public CameraFocuser(
            INavigationWheelPositionProvider positionProvider, 
            INavigationWheel navigationWheel,
            IStaticCameraTargetProvider trumpCameraTargetProvider)
        {
            Helper.AssertIsNotNull(positionProvider, navigationWheel, trumpCameraTargetProvider);

            _positionProvider = positionProvider;
            _navigationWheel = navigationWheel;
            _trumpCameraTargetProvider = trumpCameraTargetProvider;
        }

        public void FocusOnPlayerCruiser()
        {
            FocusCamera(_positionProvider.PlayerCruiserPosition);
        }

        public void FocusOnPlayerCruiserDeath()
        {
            FocusCamera(_positionProvider.PlayerCruiserDeathPosition, snapToCorners: false);
        }

        public void FocusOnPlayerNavalFactory()
        {
            FocusCamera(_positionProvider.PlayerNavalFactoryPosition);
        }

        public void FocusOnAICruiser()
        {
            FocusCamera(_positionProvider.AICruiserPosition);
        }

        public void FocusOnAICruiserDeath()
        {
            FocusCamera(_positionProvider.AICruiserDeathPosition, snapToCorners: false);
        }

        public void FocusOnAINavalFactory()
        {
            FocusCamera(_positionProvider.AINavalFactoryPosition);
        }

        public void FocusMidLeft()
        {
            FocusCamera(_positionProvider.MidLeftPosition);
        }

        public void FocusOnOverview()
        {
            FocusCamera(_positionProvider.OverviewPosition);
        }

        private void FocusCamera(Vector2 centerPosition, bool snapToCorners = true)
        {
            _navigationWheel.SetCenterPosition(centerPosition, snapToCorners);
        }

        public void FocusOnPlayerCruiserNuke()
        {
            _trumpCameraTargetProvider.SetTarget(_positionProvider.PlayerCruiserNukedTarget);
        }

        public void FocusOnAICruiserNuke()
        {
            _trumpCameraTargetProvider.SetTarget(_positionProvider.AICruiserNukedTarget);
        }
    }
}