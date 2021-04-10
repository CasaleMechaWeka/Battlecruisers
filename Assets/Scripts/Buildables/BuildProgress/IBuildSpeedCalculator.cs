using BattleCruisers.Data.Settings;

namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IBuildSpeedCalculator
    {
        float FindAIBuildSpeed(Difficulty difficulty);
        float FindIncrementalAICruiserBuildSpeed(Difficulty difficulty, int levelNum);
    }
}