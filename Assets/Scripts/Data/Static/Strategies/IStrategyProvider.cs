using BattleCruisers.Data.Static.Strategies;

// FELIX  Fix namespcae :P
namespace Assets.Scripts.Data.Static.Strategies
{
    public interface IStrategyProvider
    {
        IStrategy GetAdaptiveStrategy();
        IStrategy GetBasicStrategy();
    }
}