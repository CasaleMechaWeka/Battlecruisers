using System;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
	public abstract class UserInputNavigationWaitStep : TutorialStep
    {
		protected readonly IUserInputCameraMover _cameraMover;
		private readonly INavigationSettings _navigationSettings;

		protected UserInputNavigationWaitStep(
            ITutorialStepArgs args,
            IUserInputCameraMover cameraMover,
            INavigationSettings navigationSettings)
            : base(args)
        {
            Helper.AssertIsNotNull(cameraMover, navigationSettings);

            _cameraMover = cameraMover;
            _navigationSettings = navigationSettings;
        }

		public override void Start(Action completionCallback)
		{
			base.Start(completionCallback);
			_navigationSettings.Permission = NavigationPermission.UserInputOnly;
		}

		protected override void OnCompleted()
		{
			_navigationSettings.Permission = NavigationPermission.None;
			base.OnCompleted();
		}
	}
}
