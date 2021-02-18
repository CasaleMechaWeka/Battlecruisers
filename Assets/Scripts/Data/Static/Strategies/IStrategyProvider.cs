using BattleCruisers.Data.Static.Strategies;

namespace Assets.Scripts.Data.Static.Strategies
{
    public interface IStrategyProvider
    {
        IStrategy GetAdaptiveStrategy();
        IStrategy GetBasicStrategy();
    }
}