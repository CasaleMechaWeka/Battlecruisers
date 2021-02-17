namespace BattleCruisers.Data.Static.Strategies
{
    public enum StrategyType
    {
        Balanced, Rush, Boom
    }

    // FELIX  Implement :P
    public interface IStrategyFactory
    {
        IStrategy CreateStrategy(bool isAdaptiveAI, StrategyType strategyType);
    }
}