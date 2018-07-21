using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Drones;

namespace BattleCruisers.Cruisers
{
    public abstract class BuildableConstructionEventArgs : EventArgs
    {
		public IBuildable Buildable { get; private set; }

		protected BuildableConstructionEventArgs(IBuildable buildable)
		{
			Buildable = buildable;
		}
    }

    // FELIX  Make subclasses for IUnit & IBuildable (otherwise IBuildingMonitor has IBuildables instead of IBuildings :/)
    public class StartedConstructionEventArgs : BuildableConstructionEventArgs
    {
        public StartedConstructionEventArgs(IBuildable buildable)
            : base(buildable) { }
    }

    public class CompletedConstructionEventArgs : BuildableConstructionEventArgs
    {
        public CompletedConstructionEventArgs(IBuildable buildable) 
            : base(buildable) { }
    }

    public class BuildingDestroyedEventArgs : EventArgs
    {
        public IBuilding DestroyedBuilding { get; private set; }

        public BuildingDestroyedEventArgs(IBuilding destroyedBuilding)
        {
            DestroyedBuilding = destroyedBuilding;
        }
    }

	public interface ICruiserController
	{
        bool IsAlive { get; }
        ISlotWrapper SlotWrapper { get; }
        ISlotNumProvider SlotNumProvider { get; }
		IDroneManager DroneManager { get; }

		event EventHandler<StartedConstructionEventArgs> StartedConstruction;
		event EventHandler<CompletedConstructionEventArgs> BuildingCompleted;
		event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;

        IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot);
        void FocusOnDroneConsumer(IDroneConsumer droneConsumer);
	}
}
