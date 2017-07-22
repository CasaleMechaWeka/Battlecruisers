using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using UnityEngine;

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

	public interface ICruiser : ITarget
	{
		BuildingWrapper SelectedBuildingPrefab { get; set; }
		IDroneManager DroneManager { get; }
		IDroneConsumerProvider DroneConsumerProvider { get; }
		Direction Direction { get; }
		Vector2 Size { get; }
		float YAdjustmentInM { get; }
		Sprite Sprite { get; }

		event EventHandler<StartedConstructionEventArgs> StartedConstruction;

		bool IsSlotAvailable(SlotType slotType);
		void HighlightAvailableSlots(SlotType slotType);
		void UnhighlightSlots();
		Building ConstructBuilding(BuildingWrapper buildingPrefab, ISlot slot);
		Building ConstructSelectedBuilding(ISlot slot);
	}
}
