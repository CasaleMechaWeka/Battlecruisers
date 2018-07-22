using BattleCruisers.Buildables;
using System.Collections.ObjectModel;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public interface IInProgressBuildingMonitor
    {
        // FELIX IBuilding!  Once event args have changed :D
        ReadOnlyCollection<IBuildable> InProgressBuildings { get; }
    }
}
