﻿using System;
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
		event EventHandler<StartedConstructionEventArgs> StartedConstruction;
        event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;
        // FELIX  event SlotFreed ?

        int GetSlotCount(SlotType slotType);
		bool IsSlotAvailable(SlotType slotType);
		ISlot GetFreeSlot(SlotType slotType);
		void HighlightAvailableSlots(SlotType slotType);
		void UnhighlightSlots();
        IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot);
        void FocusOnDroneConsumer(IDroneConsumer droneConsumer);
	}
}
