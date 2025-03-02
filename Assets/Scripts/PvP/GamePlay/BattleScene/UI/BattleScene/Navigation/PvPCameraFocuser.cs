using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPCameraFocuser : ICameraFocuser
    {
        private readonly ICameraTargets _targets;
        private readonly IPvPStaticCameraTargetProvider _trumpCameraTargetProvider, _defaultCameraTargetProvider;
        private readonly ICameraTransitionSpeedManager _cameraTransitionSpeedManager;

        public PvPCameraFocuser(
            ICameraTargets targets,
            IPvPStaticCameraTargetProvider trumpCameraTargetProvider,
            IPvPStaticCameraTargetProvider defaultCameraTargetProvider,
            ICameraTransitionSpeedManager cameraTransitionSpeedManager)
        {
            PvPHelper.AssertIsNotNull(targets, trumpCameraTargetProvider, defaultCameraTargetProvider, cameraTransitionSpeedManager);

            _targets = targets;
            _trumpCameraTargetProvider = trumpCameraTargetProvider;
            _defaultCameraTargetProvider = defaultCameraTargetProvider;
            _cameraTransitionSpeedManager = cameraTransitionSpeedManager;
        }

        public void FocusOnLeftCruiser()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);

            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.PlayerCruiserTarget);
        }

        public void FocusOnLeftCruiserDeath()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.PlayerCruiserDeathTarget);
        }

        public void FocusOnLeftNavalFactory()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.PlayerNavalFactoryTarget);
        }

        public void FocusOnRightCruiser()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.EnemyCruiserTarget);
        }

        public void FocusOnRightCruiserDeath()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.EnemyCruiserDeathTarget);
        }

        public void FocusOnRightNavalFactory()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);

            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.EnemyNavalFactoryTarget);

        }

        public void FocusMidLeft()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.MidLeftTarget);
        }

        public void FocusOnOverview()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.OverviewTarget);
        }

        public void FocusOnLeftCruiserNuke()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.PlayerCruiserNukedTarget);
        }

        public void FocusOnRightCruiserNuke()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.EnemyCruiserNukedTarget);
        }
    }
}