using BattleCruisers.Data.Static.Strategies;

// FELIX  Fix namespcae :P
namespace Assets.Scripts.Data.Static.Strategies
{
    // FELIX  Rename to factory?  (And implementations...)
    public interface IStrategyProvider
    {
        IStrategy GetAdaptiveStrategy();
        IStrategy GetBasicStrategy();
    }
}