using BattleCruisers.Buildables.BuildProgress;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface IChangeCruiserBuildSpeedStepFactory
    {
        ITutorialStep CreateTutorialStep(IBuildSpeedController speedController, BuildSpeed buildSpeed);
    }
}