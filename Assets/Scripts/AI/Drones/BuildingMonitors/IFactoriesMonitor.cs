using BattleCruisers.Utils;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public interface IFactoriesMonitor : IManagedDisposable
    {
        bool AreAnyFactoriesWronglyUsingDrones { get; }
    }
}
