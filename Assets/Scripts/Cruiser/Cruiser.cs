using BattleCruisers.Buildings;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Cruisers
{
	public interface ICruiser
	{
		bool IsSlotAvailable(SlotType slotType);
		void HighlightAvailableSlots(SlotType slotType);
		void UnhighlightSlots();
	}

	public class Cruiser : FactionObject, ICruiser, IPointerClickHandler
	{
		private IDroneManager _droneManager;
		private IDictionary<SlotType, IList<Slot>> _slots;
		private GameObject _slotsWrapper;
		private SlotType? _highlightedSlotType;

		public HealthBarController healthBarController;
		public Direction direction;
		public int numOfDrones;

		void Start()
		{
			SetupSlots();
			HideAllSlots();
			healthBarController.Initialise(health);

			_droneManager = new DroneManager();
			_droneManager.NumOfDrones = numOfDrones;
		}

		private void SetupSlots()
		{
			_slots = new Dictionary<SlotType, IList<Slot>>();
			_slotsWrapper = transform.FindChild("SlotsWrapper").gameObject;
			
			Slot[] slots = GetComponentsInChildren<Slot>();
			
			foreach (Slot slot in slots)
			{
				if (!_slots.ContainsKey(slot.type))
				{
					_slots[slot.type] = new List<Slot>();
				}
				
				_slots[slot.type].Add(slot);
			}
		}

		public bool IsSlotAvailable(SlotType slotType)
		{
			return true;
		}

		public void ShowAllSlots()
		{
			_slotsWrapper.SetActive(true);
		}

		public void HideAllSlots()
		{
			_slotsWrapper.SetActive(false);
		}

		// FELIX  Disable clicking on other slots?
		// Only highlight one slot type at a time
		public void HighlightAvailableSlots(SlotType slotType)
		{
			if (_highlightedSlotType != slotType)
			{
				UnhighlightSlots();
				_highlightedSlotType = slotType;

				foreach (Slot slot in _slots[slotType])
				{
					if (slot.IsFree)
					{
						slot.IsActive = true;
					}
				}
			}
		}

		public void UnhighlightSlots()
		{
			if (_highlightedSlotType != null)
			{
				UnhighlightSlots((SlotType)_highlightedSlotType);
				_highlightedSlotType = null;
			}
		}

		private void UnhighlightSlots(SlotType slotType)
		{
			foreach (Slot slot in _slots[slotType])
			{
				slot.IsActive = false;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log("Cruiser.OnPointerClick()");
		}

		public override void TakeDamage(float damageAmount)
		{
			base.TakeDamage(damageAmount);
			healthBarController.Health = health;
		}

		public void AddBuildable(BuildableObject buildable)
		{
			buildable.StartedBuilding += Buildable_StartedBuilding;
			buildable.Destroyed += Buildable_Destroyed;
		}

		private void Buildable_StartedBuilding(object sender, EventArgs e)
		{
			BuildableObject buildable = sender as BuildableObject;
			Assert.IsNotNull(buildable);
			_droneManager.AddDroneConsumer(buildable.DroneConsumer);
		}
		
		private void Buildable_Destroyed(object sender, EventArgs e)
		{
			BuildableObject buildable = sender as BuildableObject;
			Assert.IsNotNull(buildable);
			_droneManager.RemoveDroneConsumer(buildable.DroneConsumer);
		}
	}
}
