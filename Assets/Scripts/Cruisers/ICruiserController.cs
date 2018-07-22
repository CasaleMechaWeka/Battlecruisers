using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Buildables.Units;

namespace BattleCruisers.Cruisers
{
    public abstract class BuildableConstructionEventArgs<TBuildable> : EventArgs where TBuildable : IBuildable
    {
		public TBuildable Buildable { get; private set; }

		protected BuildableConstructionEventArgs(TBuildable buildable)
		{
			Buildable = buildable;
		}
    }

    // FELIX  Move to IFactory?
    public class StartedUnitConstructionEventArgs : BuildableConstructionEventArgs<IUnit>
    {
        public StartedUnitConstructionEventArgs(IUnit unit)
            : base(unit) { }
    }

    public class CompletedUnitConstructionEventArgs : BuildableConstructionEventArgs<IUnit>
    {
        public CompletedUnitConstructionEventArgs(IUnit unit) 
            : base(unit) { }
    }

    // FELIX  Check all event subscribers, see if we want to use IBuilding intead of IBuildable now :)
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

		event EventHandler<StartedBuildingConstructionEventArgs> StartedConstruction;
		event EventHandler<CompletedBuildingConstructionEventArgs> BuildingCompleted;
		event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;

        IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot);
        void FocusOnDroneConsumer(IDroneConsumer droneConsumer);
	}
}
