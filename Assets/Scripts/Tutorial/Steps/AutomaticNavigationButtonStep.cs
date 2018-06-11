using System;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public class AutomaticNavigationButtonStep : TutorialStep
    {
        private readonly INavigationSettings _navigationSettings;
        private readonly CameraState _targetState;
        private readonly ICameraController _cameraController;

        public AutomaticNavigationButtonStep(
            ITutorialStepArgs args, 
            INavigationSettings navigationSettings, 
            CameraState targetState,
            ICameraController cameraController)
            : base(args)
        {
            Helper.AssertIsNotNull(navigationSettings, cameraController);

            _navigationSettings = navigationSettings;
            _targetState = targetState;
            _cameraController = cameraController;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _navigationSettings.Permission = NavigationPermission.TransitionsOnly;

            switch (_targetState)
            {
                case CameraState.AiCruiser:
                    _cameraController.FocusOnAiCruiser();
                    break;

                case CameraState.PlayerCruiser:
                    _cameraController.FocusOnPlayerCruiser();
                    break;

                case CameraState.LeftMid:
                    _cameraController.ShowMidLeft();
                    break;

                case CameraState.RightMid:
                    _cameraController.ShowMidRight();
                    break;

                case CameraState.Overview:
                    _cameraController.ShowFullMapView();
                    break;

                default:
                    throw new ArgumentException();
            }

            _navigationSettings.Permission = NavigationPermission.None;
            OnCompleted();
        }
    }
}
