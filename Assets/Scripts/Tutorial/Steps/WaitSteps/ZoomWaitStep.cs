using System;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
	// FELIX  Test :)

	/// <summary>
	/// Completed when the user zooms using the mouse wheel.
	/// </summary>
	public class ZoomWaitStep : UserInputNavigationWaitStep
    {
		public ZoomWaitStep(
            ITutorialStepArgs args,
			IUserInputCameraMover cameraMover,
            INavigationSettings navigationSettings)
			: base(args, cameraMover, navigationSettings)
        {
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
