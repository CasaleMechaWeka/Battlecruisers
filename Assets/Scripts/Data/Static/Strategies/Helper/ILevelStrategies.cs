namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public interface ILevelStrategies
    {
        Strategy GetAdaptiveStrategy(int levelNum);
        Strategy GetBasicStrategy(int levelNum);
    }
}
