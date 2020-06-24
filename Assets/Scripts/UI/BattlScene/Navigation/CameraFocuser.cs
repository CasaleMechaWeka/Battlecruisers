using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    // FELIX  update tests :)
    public class CameraFocuser : ICameraFocuser
    {
        private readonly ICameraTargets _targets;
        private readonly IStaticCameraTargetProvider _trumpCameraTargetProvider, _defaultCameraTargetProvider;

        public CameraFocuser(
            ICameraTargets targets, 
            IStaticCameraTargetProvider trumpCameraTargetProvider,
            IStaticCameraTargetProvider defaultCameraTargetProvider)
        {
            Helper.AssertIsNotNull(targets, trumpCameraTargetProvider, defaultCameraTargetProvider);

            _targets = targets;
            _trumpCameraTargetProvider = trumpCameraTargetProvider;
            _defaultCameraTargetProvider = defaultCameraTargetProvider;
        }

        public void FocusOnPlayerCruiser()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_targets.PlayerCruiserTarget);
        }

        public void FocusOnPlayerCruiserDeath()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _trumpCameraTargetProvider.SetTarget(_targets.PlayerCruiserDeathTarget);
        }

        public void FocusOnPlayerNavalFactory()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_targets.PlayerNavalFactoryTarget);
        }

        public void FocusOnAICruiser()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_targets.AICruiserTarget);
        }

        public void FocusOnAICruiserDeath()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _trumpCameraTargetProvider.SetTarget(_targets.AICruiserDeathTarget);
        }

        public void FocusOnAINavalFactory()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_targets.AINavalFactoryTarget);
        }

        public void FocusMidLeft()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_targets.MidLeftTarget);
        }

        public void FocusOnOverview()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_targets.OverviewTarget);
        }

        public void FocusOnPlayerCruiserNuke()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _trumpCameraTargetProvider.SetTarget(_targets.PlayerCruiserNukedTarget);
        }

        public void FocusOnAICruiserNuke()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _trumpCameraTargetProvider.SetTarget(_targets.AICruiserNukedTarget);
        }
    }
}