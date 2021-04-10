using BattleCruisers.Data.Settings;

namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IBuildProgressCalculatorFactory
    {
        IBuildProgressCalculator CreatePlayerCruiserCalculator();
        IBuildProgressCalculator CreateAICruiserCalculator(Difficulty difficulty);
        IBuildProgressCalculator CreateIncrementalAICruiserCalculator(Difficulty difficulty, int levelNum);
    }
}