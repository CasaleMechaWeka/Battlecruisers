using BattleCruisers.Data.Static.Strategies;

namespace BattleCruisers.Data.Static
{
    public interface ILevelStrategies
    {
        IStrategy GetAdaptiveStrategy(int levelNum);
        IStrategy GetBasicStrategy(int levelNum);
    }
}
