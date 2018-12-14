using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class CameraAdjustmentWaitStepFactory : TutorialFactoryBase, ITutorialStepFactory
    {
        protected CameraAdjustmentWaitStepFactory(
            ITutorialStepArgsFactory argsFactory, 
            IExplanationDismissButton explanationDismissButton, 
            IVariableDelayDeferrer deferrer, 
            ITutorialArgs tutorialArgs) 
            : base(argsFactory, explanationDismissButton, deferrer, tutorialArgs)
        {
            // empty
        }

        public ITutorialStep CreateTutorialStep()
        {
            return
                new CameraAdjustmentWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _tutorialArgs.CameraComponents.CameraAdjuster);
        }
    }
}