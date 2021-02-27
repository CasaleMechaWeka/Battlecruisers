using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class ChangeCruiserBuildSpeedStepFactory : TutorialFactoryBase, IChangeCruiserBuildSpeedStepFactory
    {
        public ChangeCruiserBuildSpeedStepFactory(ITutorialStepArgsFactory argsFactory, ILocTable tutorialStrings) 
            : base(argsFactory, tutorialStrings)
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