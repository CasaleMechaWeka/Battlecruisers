// FELIX  Fix namespcae :P
namespace BattleCruisers.Data.Static.Strategies
{
    public interface IStrategyFactory
    {
        IStrategy GetAdaptiveStrategy();
        IStrategy GetBasicStrategy();
    }
}