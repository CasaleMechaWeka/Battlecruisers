using System;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
	// FELIX  Test :)

	/// <summary>
	/// Completed when the user zooms using the mouse wheel.
	/// </summary>
	public class ZoomWaitStep : TutorialStep
    {
		private readonly IUserInputCameraMover _cameraMover;
        private readonly INavigationSettings _navigationSettings;

		public ZoomWaitStep(
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
			_cameraMover.Zoomed += _cameraMover_Zoomed;
        }

		private void _cameraMover_Zoomed(object sender, EventArgs e)
		{
			_cameraMover.Zoomed -= _cameraMover_Zoomed;
			OnCompleted();
		}
    }
}
