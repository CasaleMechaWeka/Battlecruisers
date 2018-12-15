using BattleCruisers.Tutorial.Steps.WaitSteps;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class CameraAdjustmentWaitStepFactory : TutorialFactoryBase, ITutorialStepFactory
    {
        public CameraAdjustmentWaitStepFactory(ITutorialStepArgsFactory argsFactory, ITutorialArgs tutorialArgs) 
            : base(argsFactory, tutorialArgs)
        {
            // empty
        }

        public ITutorialStep CreateStep()
        {
            return
                new CameraAdjustmentWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _tutorialArgs.CameraComponents.CameraAdjuster);
        }
    }
}