using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPCameraFocuser : IPvPCameraFocuser
    {
        private readonly IPvPCameraTargets _targets;
        private readonly IPvPStaticCameraTargetProvider _trumpCameraTargetProvider, _defaultCameraTargetProvider;
        private readonly IPvPCameraTransitionSpeedManager _cameraTransitionSpeedManager;

        public PvPCameraFocuser(
            IPvPCameraTargets targets,
            IPvPStaticCameraTargetProvider trumpCameraTargetProvider,
            IPvPStaticCameraTargetProvider defaultCameraTargetProvider,
            IPvPCameraTransitionSpeedManager cameraTransitionSpeedManager)
        {
            PvPHelper.AssertIsNotNull(targets, trumpCameraTargetProvider, defaultCameraTargetProvider, cameraTransitionSpeedManager);

            _targets = targets;
            _trumpCameraTargetProvider = trumpCameraTargetProvider;
            _defaultCameraTargetProvider = defaultCameraTargetProvider;
            _cameraTransitionSpeedManager = cameraTransitionSpeedManager;
        }

        public void FocusOnPlayerCruiser()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.PlayerCruiserTarget);
        }

        public void FocusOnPlayerCruiserDeath()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.PlayerCruiserDeathTarget);
        }

        public void FocusOnPlayerNavalFactory()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.PlayerNavalFactoryTarget);
        }

        public void FocusOnAICruiser()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.AICruiserTarget);
        }

        public void FocusOnAICruiserDeath()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.AICruiserDeathTarget);
        }

        public void FocusOnAINavalFactory()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetNormalTransitionSpeed();
            _defaultCameraTargetProvider.SetTarget(_targets.AINavalFactoryTarget);
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

        public void FocusOnPlayerCruiserNuke()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.PlayerCruiserNukedTarget);
        }

        public void FocusOnAICruiserNuke()
        {
            // Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _cameraTransitionSpeedManager.SetSlowTransitionSpeed();
            _trumpCameraTargetProvider.SetTarget(_targets.AICruiserNukedTarget);
        }
    }
}