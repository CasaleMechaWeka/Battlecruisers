using BattleCruisers.Utils;

namespace BattleCruisers.AI.Drones
{
    public interface IFactoriesMonitor : IManagedDisposable
    {
        bool AreAnyFactoriesCompleted { get; }
        bool AreAllFactoriesLowPriority { get; }
    }
}
