using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using System;

namespace BattleCruisers.Cruisers
{
    // FELIX  Move to ICruiserBuildingMonitor
    public class BuildingStartedEventArgs : EventArgs
    {
        public IBuilding StartedBuilding { get; }

        public BuildingStartedEventArgs(IBuilding startedBuilding)
        {
            StartedBuilding = startedBuilding;
        }
    }

    public class BuildingCompletedEventArgs : EventArgs
    {
        public IBuilding CompletedBuilding { get; }

        public BuildingCompletedEventArgs(IBuilding completedBuilding)
        {
            CompletedBuilding = completedBuilding;
        }
    }

	public interface ICruiserController
	{
        bool IsAlive { get; }
        ISlotAccessor SlotAccessor { get; }
        ISlotHighlighter SlotHighlighter { get; }
        ISlotNumProvider SlotNumProvider { get; }
		IDroneManager DroneManager { get; }
        IDroneFocuser DroneFocuser { get; }
        ICruiserBuildingMonitor BuildingMonitor { get; }
        ICruiserUnitMonitor UnitMonitor { get; }

        event EventHandler<BuildingStartedEventArgs> BuildingStarted;

        IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot);
	}
}
