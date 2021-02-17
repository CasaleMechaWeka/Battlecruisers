namespace BattleCruisers.Data.Static.Strategies
{
    public interface ILevelStrategies
    {
        IStrategy GetAdaptiveStrategy(int levelNum);
        IStrategy GetBasicStrategy(int levelNum);
    }
}
