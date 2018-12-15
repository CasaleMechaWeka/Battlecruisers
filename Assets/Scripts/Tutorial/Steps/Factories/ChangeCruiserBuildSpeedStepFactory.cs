using BattleCruisers.Buildables.BuildProgress;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ChangeCruiserBuildSpeedStepFactory : TutorialFactoryBase, IChangeCruiserBuildSpeedStepFactory
    {
        public ChangeCruiserBuildSpeedStepFactory(ITutorialStepArgsFactory argsFactory, ITutorialArgs tutorialArgs) 
            : base(argsFactory, tutorialArgs)
        {
            // empty
        }

        public ITutorialStep CreateStep(IBuildSpeedController speedController, BuildSpeed buildSpeed)
        {
            return
                new ChangeCruiserBuildSpeedStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    speedController,
                    buildSpeed);
        }
    }
}