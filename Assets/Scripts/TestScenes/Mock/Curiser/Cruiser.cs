using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Mock
{
	public class Cruiser : ICruiser
	{
		public Building SelectedBuildingPrefab { get; set; }
		public IDroneManager DroneManager { get; set; }
		public IDroneConsumerProvider DroneConsumerProvider { get; set; }
		public Direction Direction { get; set; }
		public GameObject GameObject { get; set; }

		public bool IsSlotAvailable(SlotType slotType)
		{
			throw new NotImplementedException();
		}

		public void HighlightAvailableSlots(SlotType slotType)
		{
			throw new NotImplementedException();
		}

		public void UnhighlightSlots()
		{
			throw new NotImplementedException();
		}

		public Building ConstructBuilding(Building buildingPrefab, ISlot slot)
		{
			throw new NotImplementedException();
		}

		public Building ConstructSelectedBuilding(ISlot slot)
		{
			throw new NotImplementedException();
		}
	}
}
