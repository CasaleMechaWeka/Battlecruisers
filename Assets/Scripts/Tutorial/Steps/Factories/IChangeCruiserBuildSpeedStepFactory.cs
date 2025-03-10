using BattleCruisers.Buildables.BuildProgress;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public interface IChangeCruiserBuildSpeedStepFactory
    {
        ITutorialStep CreateStep(IBuildSpeedController speedController, BuildSpeed buildSpeed);
    }
}