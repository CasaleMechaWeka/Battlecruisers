using BattleCruisers.Buildables.Buildings;
using System.Collections.ObjectModel;

namespace BattleCruisers.AI.Drones.BuildingMonitors
{
    public interface IInProgressBuildingMonitor
    {
        ReadOnlyCollection<IBuilding> InProgressBuildings { get; }
    }
}
