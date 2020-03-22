using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
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
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            FocusCamera(_positionProvider.PlayerCruiserPosition);
        }

        public void FocusOnPlayerCruiserDeath()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            FocusCamera(_positionProvider.PlayerCruiserDeathPosition, snapToCorners: false);
        }

        public void FocusOnPlayerNavalFactory()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            FocusCamera(_positionProvider.PlayerNavalFactoryPosition, snapToCorners: false);
        }

        public void FocusOnAICruiser()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            FocusCamera(_positionProvider.AICruiserPosition);
        }

        public void FocusOnAICruiserDeath()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            FocusCamera(_positionProvider.AICruiserDeathPosition, snapToCorners: false);
        }

        public void FocusOnAINavalFactory()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            FocusCamera(_positionProvider.AINavalFactoryPosition, snapToCorners: false);
        }

        public void FocusMidLeft()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            FocusCamera(_positionProvider.MidLeftPosition);
        }

        public void FocusOnOverview()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            FocusCamera(_positionProvider.OverviewPosition);
        }

        private void FocusCamera(Vector2 centerPosition, bool snapToCorners = true)
        {
            Logging.Log(Tags.CAMERA_FOCUSER, $"centerPosition: {centerPosition}  snapToCorners: {snapToCorners}");
            _navigationWheel.SetCenterPosition(centerPosition, snapToCorners);
        }

        public void FocusOnPlayerCruiserNuke()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _trumpCameraTargetProvider.SetTarget(_positionProvider.PlayerCruiserNukedTarget);
        }

        public void FocusOnAICruiserNuke()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _trumpCameraTargetProvider.SetTarget(_positionProvider.AICruiserNukedTarget);
        }
    }
}