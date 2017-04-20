using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using BattleCruisers.UI.ProgressBars;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Cruisers
{
	public interface ICruiser
	{
		Building SelectedBuildingPrefab { get; set; }
		IDroneManager DroneManager { get; }
		IDroneConsumerProvider DroneConsumerProvider { get; }

		bool IsSlotAvailable(SlotType slotType);
		void HighlightAvailableSlots(SlotType slotType);
		void UnhighlightSlots();
		Building ConstructBuilding(Building buildingPrefab, ISlot slot);
		Building ConstructSelectedBuilding(ISlot slot);
	}

	public class Cruiser : FactionObject, ICruiser, IPointerClickHandler
	{
		private IDictionary<SlotType, IList<Slot>> _slots;
		private GameObject _slotsWrapper;
		private SlotType? _highlightedSlotType;

		public HealthBarController healthBarController;
		public UIManager uiManager;
		public Cruiser enemyCruiser;
		public BuildableFactory buildableFactory;
		public Direction direction;
		public int numOfDrones;
		public Faction faction;

		public Building SelectedBuildingPrefab { get; set; }
		public IDroneManager DroneManager { get; private set; }
		public IDroneConsumerProvider DroneConsumerProvider { get; private set; }

		void Start()
		{
			Faction = faction;

			SetupSlots();
			HideAllSlots();

			healthBarController.Initialise(this);
		}

		public void Initialise(IDroneManager droneManager, IDroneConsumerProvider droneConsumerProvider)
		{
			Assert.IsNotNull(droneManager);
			Assert.IsNotNull(droneConsumerProvider);

			DroneManager = droneManager;
			DroneManager.NumOfDrones = numOfDrones;
			DroneConsumerProvider = droneConsumerProvider;
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

		public Building ConstructBuilding(Building buildingPrefab, ISlot slot)
		{
			SelectedBuildingPrefab = buildingPrefab;
			return ConstructSelectedBuilding(slot);
		}

		public Building ConstructSelectedBuilding(ISlot slot)
		{
			Assert.IsNotNull(SelectedBuildingPrefab);
			Assert.AreEqual(SelectedBuildingPrefab.slotType, slot.Type);

			Building building = buildableFactory.CreateBuilding(SelectedBuildingPrefab);
			building.Initialise(Faction, uiManager, this, enemyCruiser, buildableFactory);
			slot.Building = building;

			// Only show build menu for player's cruiser
			if (Faction == Faction.Blues)
			{
				uiManager.ShowBuildingGroups();
			}

			building.StartConstruction();
			return building;
		}

		public ISlot GetFreeSlot(SlotType slotType)
		{
			return _slots[slotType].FirstOrDefault(slot => slot.IsFree);
		}
	}
}
