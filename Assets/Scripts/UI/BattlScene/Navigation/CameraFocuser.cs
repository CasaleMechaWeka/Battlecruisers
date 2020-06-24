using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    // FELIX  update tests :)
    public class CameraFocuser : ICameraFocuser
    {
        private readonly INavigationWheelPositionProvider _positionProvider;
        private readonly IStaticCameraTargetProvider _trumpCameraTargetProvider, _defaultCameraTargetProvider;

        public CameraFocuser(
            INavigationWheelPositionProvider positionProvider, 
            IStaticCameraTargetProvider trumpCameraTargetProvider,
            IStaticCameraTargetProvider defaultCameraTargetProvider)
        {
            Helper.AssertIsNotNull(positionProvider, trumpCameraTargetProvider, defaultCameraTargetProvider);

            _positionProvider = positionProvider;
            _trumpCameraTargetProvider = trumpCameraTargetProvider;
            _defaultCameraTargetProvider = defaultCameraTargetProvider;
        }

        public void FocusOnPlayerCruiser()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_positionProvider.PlayerCruiserTarget);
        }

        public void FocusOnPlayerCruiserDeath()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _trumpCameraTargetProvider.SetTarget(_positionProvider.PlayerCruiserDeathTarget);
        }

        public void FocusOnPlayerNavalFactory()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_positionProvider.PlayerNavalFactoryTarget);
        }

        public void FocusOnAICruiser()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_positionProvider.AICruiserTarget);
        }

        public void FocusOnAICruiserDeath()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _trumpCameraTargetProvider.SetTarget(_positionProvider.AICruiserDeathTarget);
        }

        public void FocusOnAINavalFactory()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_positionProvider.AINavalFactoryTarget);
        }

        public void FocusMidLeft()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_positionProvider.MidLeftTarget);
        }

        public void FocusOnOverview()
        {
            Logging.LogMethod(Tags.CAMERA_FOCUSER);
            _defaultCameraTargetProvider.SetTarget(_positionProvider.OverviewTarget);
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