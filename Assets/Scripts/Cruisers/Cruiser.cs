﻿using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Cruisers
{
    public class Cruiser : Target, ICruiser, IPointerClickHandler, IComparableItem
	{
		private HealthBarController _healthBarController;
		private IUIManager _uiManager;
		private ICruiser _enemyCruiser;
		private SpriteRenderer _renderer;

		public int numOfDrones;
		public float yAdjustmentInM;
        public string description;
		
		// ITarget
		public override TargetType TargetType { get { return TargetType.Cruiser; } }
		
		// IComparableItem
		public string Description { get { return description; } }
        public string Name { get { return name; } }

        // ICruiser
        public IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        public IDroneManager DroneManager { get; private set; }
        public IDroneConsumerProvider DroneConsumerProvider { get; private set; }
        public Direction Direction { get; private set; }
        public Vector2 Size { get { return _renderer.bounds.size; } }
        public float YAdjustmentInM { get { return yAdjustmentInM; } }
        public Sprite Sprite { get { return _renderer.sprite; } }
        public ISlotWrapper SlotWrapper { get; private set; }
		public IFactoryProvider FactoryProvider { get; private set; }

		public event EventHandler<StartedConstructionEventArgs> StartedConstruction;
        public event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;

        public override void StaticInitialise()
		{
			base.StaticInitialise();

			_renderer = GetComponent<SpriteRenderer>();
			Assert.IsNotNull(_renderer);
        }

        public void Initialise(Faction faction, ICruiser enemyCruiser, HealthBarController healthBarController,
            IUIManager uiManager, IDroneManager droneManager, IDroneConsumerProvider droneConsumerProvider, 
            IFactoryProvider factoryProvider, Direction facingDirection)
        {
            Helper.AssertIsNotNull(enemyCruiser, healthBarController, uiManager, droneManager, droneConsumerProvider, factoryProvider);

            Faction = faction;
            _enemyCruiser = enemyCruiser;
            _healthBarController = healthBarController;
            _uiManager = uiManager;
            DroneManager = droneManager;
            DroneManager.NumOfDrones = numOfDrones;
            DroneConsumerProvider = droneConsumerProvider;
            FactoryProvider = factoryProvider;
            Direction = facingDirection;

            _healthBarController.Initialise(this);
			
			SlotWrapper slotWrapper = GetComponentInChildren<SlotWrapper>(includeInactive: true);
			Assert.IsNotNull(slotWrapper);
			slotWrapper.StaticInitialise();
			SlotWrapper = slotWrapper;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Logging.Log(Tags.CRUISER, "Cruiser.OnPointerClick()");
		}

        public IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot)
        {
			SelectedBuildingPrefab = buildingPrefab;
			return ConstructSelectedBuilding(slot);
		}

        public IBuilding ConstructSelectedBuilding(ISlot slot)
		{
			Assert.IsNotNull(SelectedBuildingPrefab);
            Assert.AreEqual(SelectedBuildingPrefab.Buildable.SlotType, slot.Type);

            IBuilding building = FactoryProvider.PrefabFactory.CreateBuilding(SelectedBuildingPrefab);
			building.Initialise(this, _enemyCruiser, _uiManager, FactoryProvider);
			slot.Building = building;
            building.Destroyed += Building_Destroyed;

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

        public void FocusOnDroneConsumer(IDroneConsumer droneConsumer)
        {
            if (DroneManager.NumOfDrones > droneConsumer.NumOfDrones)
            {
                DroneManager.ToggleDroneConsumerFocus(droneConsumer);
            }
        }

        private void Building_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Building_Destroyed;

            if (BuildingDestroyed != null)
            {
				IBuilding destroyedBuilding = e.DestroyedTarget as IBuilding;
				Assert.IsNotNull(destroyedBuilding);
				BuildingDestroyed.Invoke(this, new BuildingDestroyedEventArgs(destroyedBuilding));
            }
        }
    }
}
