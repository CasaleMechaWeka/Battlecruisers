using System.Collections.ObjectModel;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public interface IFactoriesMonitor
    {
        ReadOnlyCollection<IFactoryMonitor> CompletedFactories { get; }
    }
}
