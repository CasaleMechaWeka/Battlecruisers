namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public enum StrategyType
    {
        Balanced, Rush, Boom
    }

    public interface IStrategyFactory
    {
        Strategy GetAdaptiveStrategy();
        Strategy GetBasicStrategy();
    }
}