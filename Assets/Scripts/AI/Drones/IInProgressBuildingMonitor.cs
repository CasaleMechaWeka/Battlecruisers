using BattleCruisers.Buildables;
using System.Collections.ObjectModel;

namespace BattleCruisers.AI.Drones
{
    public interface IInProgressBuildingMonitor
    {
        // FELIX IBuilding!  Once event args have changed :D
        ReadOnlyCollection<IBuildable> InProgressBuildings { get; }
    }
}
