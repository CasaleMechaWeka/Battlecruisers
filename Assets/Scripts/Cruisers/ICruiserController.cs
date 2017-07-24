using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Drones;

namespace BattleCruisers.Cruisers
{
	public class StartedConstructionEventArgs : EventArgs
	{
		public IBuildable Buildable { get; private set; }

		public StartedConstructionEventArgs(IBuildable buildable)
		{
			Buildable = buildable;
		}
	}

	public interface ICruiserController
	{
		event EventHandler<StartedConstructionEventArgs> StartedConstruction;

        // FELIX  event SlotFreed ?

		bool IsSlotAvailable(SlotType slotType);
		ISlot GetFreeSlot(SlotType slotType);
		void HighlightAvailableSlots(SlotType slotType);
		void UnhighlightSlots();
        IBuildable ConstructBuilding(IBuildableWrapper<Building> buildingPrefab, ISlot slot);
        void FocusOnDroneConsumer(IDroneConsumer droneConsumer);
	}
}
