using BattleCruisers.Data.Static.Strategies;

// FELIX  fix namespace :)
namespace BattleCruisers.Data.Static
{
    public interface ILevelStrategies
    {
        IStrategy GetAdaptiveStrategy(int levelNum);
        IStrategy GetBasicStrategy(int levelNum);
    }
}
