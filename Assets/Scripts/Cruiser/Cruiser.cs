using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI;
using BattleCruisers.UI.ProgressBars;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Cruisers
{
	public class Cruiser : Target, ICruiser, IPointerClickHandler
	{
		private IDictionary<SlotType, IList<Slot>> _slots;
		private GameObject _slotsWrapper;
		private SlotType? _highlightedSlotType;
		private ITargetsFactory _targetsFactory;

		public HealthBarController healthBarController;
		public UIManager uiManager;
		public Cruiser enemyCruiser;
		public BuildableFactory buildableFactory;

		// FELIX  Remove.  Instead pass in Initialise().  
		// FELIX  Perhaps move IBuildable.FacingDirection up to ITarget to avoid duplication here?
		// FELIX  Expose for child buildables!
		public Direction direction;

		public int numOfDrones;
		public Faction faction;

		public Building SelectedBuildingPrefab { get; set; }
		public IDroneManager DroneManager { get; private set; }
		public IDroneConsumerProvider DroneConsumerProvider { get; private set; }
		public Direction Direction { get { return direction; } }
		public override TargetType TargetType { get { return TargetType.Cruiser; } }

		public event EventHandler<StartedConstructionEventArgs> StartedConstruction;

		void Start()
		{
			Faction = faction;

			SetupSlots();
			HideAllSlots();

			healthBarController.Initialise(this);
		}

		public void Initialise(IDroneManager droneManager, IDroneConsumerProvider droneConsumerProvider, ITargetsFactory targetsFactory)
		{
			Assert.IsNotNull(droneManager);
			Assert.IsNotNull(droneConsumerProvider);
			Assert.IsNotNull(targetsFactory);

			DroneManager = droneManager;
			DroneManager.NumOfDrones = numOfDrones;
			DroneConsumerProvider = droneConsumerProvider;
			_targetsFactory = targetsFactory;
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
			Logging.Log(Tags.CRUISER, "Cruiser.OnPointerClick()");
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
			building.Initialise(Faction, uiManager, this, enemyCruiser, buildableFactory, _targetsFactory);
			slot.Building = building;

			// Only show build menu for player's cruiser
			if (Faction == Faction.Blues)
			{
				uiManager.ShowBuildingGroups();
			}

			building.StartConstruction();

			if (StartedConstruction != null)
			{
				StartedConstruction.Invoke(this, new StartedConstructionEventArgs(building));
			}

			return building;
		}

		public ISlot GetFreeSlot(SlotType slotType)
		{
			return _slots[slotType].FirstOrDefault(slot => slot.IsFree);
		}
	}
}
