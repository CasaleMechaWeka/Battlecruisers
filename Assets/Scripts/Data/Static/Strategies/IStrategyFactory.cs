namespace BattleCruisers.Data.Static.Strategies
{
    public interface IStrategyFactory
    {
        IStrategy GetAdaptiveStrategy();
        IStrategy GetBasicStrategy();
    }
}