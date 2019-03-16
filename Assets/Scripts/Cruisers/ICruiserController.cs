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

    public class StartedBuildingConstructionEventArgs : BuildableConstructionEventArgs<IBuilding>
    {
        public StartedBuildingConstructionEventArgs(IBuilding building)
            : base(building) { }
    }

    public class CompletedBuildingConstructionEventArgs : BuildableConstructionEventArgs<IBuilding>
    {
        public CompletedBuildingConstructionEventArgs(IBuilding building)
            : base(building) { }
    }

    public class BuildingDestroyedEventArgs : EventArgs
    {
        public IBuilding DestroyedBuilding { get; }

        public BuildingDestroyedEventArgs(IBuilding destroyedBuilding)
        {
            DestroyedBuilding = destroyedBuilding;
        }
    }

	public interface ICruiserController : IUnitConstructionMonitor
	{
        bool IsAlive { get; }
        ISlotAccessor SlotAccessor { get; }
        ISlotHighlighter SlotHighlighter { get; }
        ISlotNumProvider SlotNumProvider { get; }
		IDroneManager DroneManager { get; }
        IDroneFocuser DroneFocuser { get; }

        event EventHandler<StartedBuildingConstructionEventArgs> BuildingStarted;
		event EventHandler<CompletedBuildingConstructionEventArgs> BuildingCompleted;
		event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;

        IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot);
	}
}
