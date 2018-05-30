using System;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
    // FELIX  Test :)

    /// <summary>
    /// Completed when the user scrolls by moving the mouse to the screen edge.
    /// </summary>
    public class ScrollWaitStep : UserInputNavigationWaitStep
    {
		public ScrollWaitStep(
            ITutorialStepArgs args,
            IUserInputCameraMover cameraMover,
            INavigationSettings navigationSettings)
            : base(args, cameraMover, navigationSettings)
        {
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
			_cameraMover.Scrolled += _cameraMover_Scrolled;
        }

        private void _cameraMover_Scrolled(object sender, EventArgs e)
        {
			_cameraMover.Scrolled -= _cameraMover_Scrolled;
            OnCompleted();
        }
    }
}
