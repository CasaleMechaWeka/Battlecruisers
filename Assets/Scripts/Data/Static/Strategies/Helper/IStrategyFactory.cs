namespace BattleCruisers.Data.Static.Strategies.Helper
{
    public interface IStrategyFactory
    {
        IStrategy GetAdaptiveStrategy();
        IStrategy GetBasicStrategy();
    }
}