using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using System;

namespace BattleCruisers.Cruisers
{
    // FELIX  Remove.  Benefit is minimal :P
    public abstract class BuildableConstructionEventArgs<TBuildable> : EventArgs where TBuildable : IBuildable
    {
		public TBuildable Buildable { get; }

		protected BuildableConstructionEventArgs(TBuildable buildable)
		{
			Buildable = buildable;
		}
    }

    // FELIX  Move to ICruiserBuildingMonitor
    public class BuildingStartedEventArgs : BuildableConstructionEventArgs<IBuilding>
    {
        public BuildingStartedEventArgs(IBuilding building)
            : base(building) { }
    }

    public class BuildingCompletedEventArgs : BuildableConstructionEventArgs<IBuilding>
    {
        public BuildingCompletedEventArgs(IBuilding building)
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
        ICruiserBuildingMonitor BuildingMonitor { get; }
        ICruiserUnitMonitor UnitMonitor { get; }

        event EventHandler<BuildingStartedEventArgs> BuildingStarted;

        IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot);
	}
}
