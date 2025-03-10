namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public interface ILevelStrategies
    {
        IStrategy GetAdaptiveStrategy(int levelNum);
        IStrategy GetBasicStrategy(int levelNum);
    }
}
