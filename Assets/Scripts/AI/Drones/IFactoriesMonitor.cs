using BattleCruisers.Utils;

namespace BattleCruisers.AI.Drones
{
    public interface IFactoriesMonitor : IManagedDisposable
    {
        bool AreAnyFactoriesWronglyUsingDrones { get; }
    }
}
