using BattleCruisers.Data.Settings;

namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IBuildProgressCalculatorFactory
    {
        IBuildProgressCalculator CreatePlayerCruiserCalculator();
        IBuildProgressCalculator CreateAICruiserCalculator(Difficulty difficulty);
    }
}