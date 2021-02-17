namespace BattleCruisers.Data.Static.Strategies
{
    public enum StrategyType
    {
        Balanced, Rush, Boom
    }

    public interface IStrategyFactory
    {
        IStrategy CreateStrategy(bool isAdaptiveAI, StrategyType strategyType);
    }
}