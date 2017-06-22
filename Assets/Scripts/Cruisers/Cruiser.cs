using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
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
		private HealthBarController _healthBarController;
		private UIManager _uiManager;
		private Cruiser _enemyCruiser;
		private IDictionary<SlotType, IList<Slot>> _slots;
		private GameObject _slotsWrapper;
		private SlotType? _highlightedSlotType;
		private IFactoryProvider _factoryProvider;
		private SpriteRenderer _renderer;

		public int numOfDrones;
		public string description;
		public float yAdjustmentInM;

		public BuildingWrapper SelectedBuildingPrefab { get; set; }
		public IDroneManager DroneManager { get; private set; }
		public IDroneConsumerProvider DroneConsumerProvider { get; private set; }
		public Direction Direction { get; private set; }
		public override TargetType TargetType { get { return TargetType.Cruiser; } }
		public Vector2 Size { get { return _renderer.bounds.size; } }
		public float YAdjustmentInM { get { return yAdjustmentInM; } }
		public Sprite Sprite { get { return _renderer.sprite; } }

		public event EventHandler<StartedConstructionEventArgs> StartedConstruction;

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_renderer = GetComponent<SpriteRenderer>();
			Assert.IsNotNull(_renderer);

			SetupSlots();
			HideAllSlots();
		}

		public void Initialise(Faction faction, Cruiser enemyCruiser, HealthBarController healthBarController,
			UIManager uiManager, IDroneManager droneManager, IDroneConsumerProvider droneConsumerProvider, 
			IFactoryProvider factoryProvider, Direction facingDirection)
		{
			Assert.IsNotNull(enemyCruiser);
			Assert.IsNotNull(healthBarController);
			Assert.IsNotNull(uiManager);
			Assert.IsNotNull(droneManager);
			Assert.IsNotNull(droneConsumerProvider);
			Assert.IsNotNull(factoryProvider);

			Faction = faction;
			_enemyCruiser = enemyCruiser;
			_healthBarController = healthBarController;
			_uiManager = uiManager;
			DroneManager = droneManager;
			DroneManager.NumOfDrones = numOfDrones;
			DroneConsumerProvider = droneConsumerProvider;
			_factoryProvider = factoryProvider;
			Direction = facingDirection;

			_healthBarController.Initialise(this);
		}

		private void SetupSlots()
		{
			_slots = new Dictionary<SlotType, IList<Slot>>();
			_slotsWrapper = transform.Find("SlotsWrapper").gameObject;
			
			Slot[] slots = GetComponentsInChildren<Slot>(includeInactive: true);
			
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

		public Building ConstructBuilding(BuildingWrapper buildingPrefab, ISlot slot)
		{
			SelectedBuildingPrefab = buildingPrefab;
			return ConstructSelectedBuilding(slot);
		}

		public Building ConstructSelectedBuilding(ISlot slot)
		{
			Assert.IsNotNull(SelectedBuildingPrefab);
			Assert.AreEqual(SelectedBuildingPrefab.Building.slotType, slot.Type);

			Building building = _factoryProvider.PrefabFactory.CreateBuilding(SelectedBuildingPrefab);
			building.Initialise(this, _enemyCruiser, _uiManager, _factoryProvider);
			slot.Building = building;

			// Only show build menu for player's cruiser
			if (Faction == Faction.Blues)
			{
				_uiManager.ShowBuildingGroups();
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

		public int GetSlotCount(SlotType slotType)
		{
			return _slots[slotType].Count;
		}
	}
}
