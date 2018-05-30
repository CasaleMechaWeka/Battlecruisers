using System;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
	/// <summary>
	/// Completed when camera reaches the specified state.
	/// </summary>
	public class NavigationTransitionWaitStep : TutorialStep
    {
		private readonly ICameraMover _cameraMover;
		private readonly CameraState _targetState;
		private readonly INavigationSettings _navigationSettings;

		public NavigationTransitionWaitStep(
			ITutorialStepArgs args, 
			ICameraMover cameraMover, 
			CameraState targetState,
			INavigationSettings navigationSettings)
            : base(args)
        {
			Helper.AssertIsNotNull(cameraMover, targetState, navigationSettings);

			_cameraMover = cameraMover;
			_targetState = targetState;
			_navigationSettings = navigationSettings;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

			if (_cameraMover.State == _targetState)
			{
				OnCompleted();
			}
			else
			{
				_navigationSettings.Permission = NavigationPermission.TransitionsOnly;
				_cameraMover.StateChanged += _cameraMover_StateChanged;
			}
        }

        private void _cameraMover_StateChanged(object sender, CameraStateChangedArgs e)
        {
            if (e.NewState == _targetState)
            {
				_navigationSettings.Permission = NavigationPermission.None;
				_cameraMover.StateChanged -= _cameraMover_StateChanged;
				OnCompleted();
			}
		}
    }
}
