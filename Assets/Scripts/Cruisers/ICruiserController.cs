using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using System;

namespace BattleCruisers.Cruisers
{
    public abstract class BuildableConstructionEventArgs<TBuildable> : EventArgs where TBuildable : IBuildable
    {
		public TBuildable Buildable { get; }

		protected BuildableConstructionEventArgs(TBuildable buildable)
		{
			Buildable = buildable;
		}
    }

    // FELIX  Rename to StartedBuildingEventArgs :)
    public class StartedBuildingConstructionEventArgs : BuildableConstructionEventArgs<IBuilding>
    {
        public StartedBuildingConstructionEventArgs(IBuilding building)
            : base(building) { }
    }

    // FELIX  Move to ICruiserBuildingMonitor
    // FELIX  Rename to BuildingCompletedEventArgs :)
    public class CompletedBuildingConstructionEventArgs : BuildableConstructionEventArgs<IBuilding>
    {
        public CompletedBuildingConstructionEventArgs(IBuilding building)
            : base(building) { }
    }

	public interface ICruiserController
	{
        bool IsAlive { get; }
        ISlotAccessor SlotAccessor { get; }
        ISlotHighlighter SlotHighlighter { get; }
        ISlotNumProvider SlotNumProvider { get; }
		IDroneManager DroneManager { get; }
        IDroneFocuser DroneFocuser { get; }
        ICruiserUnitMonitor UnitMonitor { get; }

        event EventHandler<StartedBuildingConstructionEventArgs> BuildingStarted;
        
        // FELIX  Replace with ICruiserBuildingMonitor getter :)
		event EventHandler<CompletedBuildingConstructionEventArgs> BuildingCompleted;
		event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;

        IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot);
	}
}
