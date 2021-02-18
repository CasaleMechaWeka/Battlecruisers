// FELIX  Fix namespcae :P
namespace BattleCruisers.Data.Static.Strategies
{
    // FELIX  Rename to factory?  (And implementations...)
    public interface IStrategyProvider
    {
        IStrategy GetAdaptiveStrategy();
        IStrategy GetBasicStrategy();
    }
}