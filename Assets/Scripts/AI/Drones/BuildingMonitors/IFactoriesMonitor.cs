using System.Collections.Generic;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public interface IFactoriesMonitor
    {
        IReadOnlyCollection<IFactoryMonitor> CompletedFactories { get; }
    }
}
