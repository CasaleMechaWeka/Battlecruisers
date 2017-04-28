using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables;
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
		public bool IsDestroyed { get; set; }
		public float Health { get; set; }
		public Faction Faction { get; set; }
		public Building SelectedBuildingPrefab { get; set; }
		public IDroneManager DroneManager { get; set; }
		public IDroneConsumerProvider DroneConsumerProvider { get; set; }
		public Direction Direction { get; set; }
		public GameObject GameObject { get; set; }

		#pragma warning disable 67  // Unused event
		public event EventHandler Destroyed;
		public event EventHandler<BattleCruisers.Buildables.HealthChangedEventArgs> HealthChanged;
		public event EventHandler FullyRepaired;
		#pragma warning restore 67  // Unused event

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

		public void TakeDamage(float damageAmount)
		{
			throw new NotImplementedException();
		}

		public void Repair(float repairAmount)
		{
			throw new NotImplementedException();
		}
	}
}
