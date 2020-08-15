using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class CameraFocuser : ICameraFocuser
    {
        private readonly ICameraTargets _targets;
        private readonly IStaticCameraTargetProvider _trumpCameraTargetProvider, _defaultCameraTargetProvider;
        private readonly ICameraTransitionSpeedManager _cameraTransitionSpeedManager;

        public CameraFocuser(
            ICameraTargets targets, 
            IStaticCameraTargetProvider trumpCameraTargetProvider,
            IStaticCameraTargetProvider defaultCameraTargetProvider,
            ICameraTransitionSpeedManager cameraTransitionSpeedManager)
        {
            Helper.AssertIsNotNull(targets, trumpCameraTargetProvider, defaultCameraTargetProvider, cameraTransitionSpeedManager);

            _targets = targets;
            _trumpCameraTargetProvider = trumpCameraTargetProvider;
            _defaultCameraTargetProvider = defaultCameraTargetProvider;
            _cameraTransitionSpeedManager = cameraTransitionSpeedManager;
        }

        public void FocusOnPlayerCruiser()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.PlayerCruiserTarget);
        }

        public void FocusOnPlayerCruiserDeath()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.PlayerCruiserDeathTarget);
        }

        public void FocusOnPlayerNavalFactory()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.PlayerNavalFactoryTarget);
        }

        public void FocusOnAICruiser()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.AICruiserTarget);
        }

        public void FocusOnAICruiserDeath()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.AICruiserDeathTarget);
        }

        public void FocusOnAINavalFactory()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.AINavalFactoryTarget);
        }

        public void FocusMidLeft()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.MidLeftTarget);
        }

        public void FocusOnOverview()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.OverviewTarget);
        }

        public void FocusOnPlayerCruiserNuke()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.PlayerCruiserNukedTarget);
        }

        public void FocusOnAICruiserNuke()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.AICruiserNukedTarget);
        }
    }
}