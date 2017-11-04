using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Drones;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.UIWrappers;
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
		public Sprite Sprite { get { return _renderer.sprite; } }

        // ICruiser
        public IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        public IDroneManager DroneManager { get; private set; }
        public IDroneConsumerProvider DroneConsumerProvider { get; private set; }
        public Direction Direction { get; private set; }
        public Vector2 Size { get { return _renderer.bounds.size; } }
        public float YAdjustmentInM { get { return yAdjustmentInM; } }
        public ISlotWrapper SlotWrapper { get; private set; }
        public IFactoryProvider FactoryProvider { get; private set; }
        private FogOfWar _fog;
        public IFogOfWar Fog { get { return _fog; } }
		public IRepairManager RepairManager { get; private set; }
        public int NumOfDrones { get { return numOfDrones; } }
        public IGameObject HealthBar { get { return _healthBarController; } }

        public event EventHandler<StartedConstructionEventArgs> StartedConstruction;
        public event EventHandler<CompletedConstructionEventArgs> BuildingCompleted;
        public event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;

        public override void StaticInitialise()
		{
			base.StaticInitialise();

			_renderer = GetComponent<SpriteRenderer>();
			Assert.IsNotNull(_renderer);

			SlotWrapper slotWrapper = GetComponentInChildren<SlotWrapper>(includeInactive: true);
			Assert.IsNotNull(slotWrapper);
            slotWrapper.Initialise(parentCruiser: this);
			SlotWrapper = slotWrapper;

            _fog = GetComponentInChildren<FogOfWar>(includeInactive: true);
            Assert.IsNotNull(_fog);
        }

        protected override ITextMesh GetRepairDroneNumText()
        {
            TextMesh numOfRepairDronesText = GetComponentInChildren<TextMesh>(includeInactive: true);
            Assert.IsNotNull(numOfRepairDronesText);
            return new TextMeshWrapper(numOfRepairDronesText);
        }

        public void Initialise(ICruiserArgs args)
        {
            Faction = args.Faction;
            _enemyCruiser = args.EnemyCruiser;
            _healthBarController = args.HealthBarController;
            _uiManager = args.UiManager;
            DroneManager = args.DroneManager;
            DroneManager.NumOfDrones = numOfDrones;
            DroneConsumerProvider = args.DroneConsumerProvider;
            FactoryProvider = args.FactoryProvider;
            Direction = args.FacingDirection;
			
            args.RepairManager.Initialise(this);
            RepairManager = args.RepairManager;

            _healthBarController.Initialise(this);
            _fog.Initialise(args.ShouldShowFog);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
            _uiManager.ShowCruiserDetails(this);
		}

        public IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot)
        {
            Logging.Log(Tags.CRUISER, "ConstructBuilding() " + buildingPrefab.Buildable.Name);

			SelectedBuildingPrefab = buildingPrefab;
			return ConstructSelectedBuilding(slot);
		}

        public IBuilding ConstructSelectedBuilding(ISlot slot)
		{
			Assert.IsNotNull(SelectedBuildingPrefab);
            Assert.AreEqual(SelectedBuildingPrefab.Buildable.SlotType, slot.Type);

            IBuilding building = FactoryProvider.PrefabFactory.CreateBuilding(SelectedBuildingPrefab);
            building.Initialise(this, _enemyCruiser, _uiManager, FactoryProvider, slot);
			slot.Building = building;

			building.CompletedBuildable += Building_CompletedBuildable;
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

        private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            IBuilding completedBuilding = sender.Parse<IBuilding>();
            completedBuilding.CompletedBuildable -= Building_CompletedBuildable;

            if (BuildingCompleted != null)
            {
                BuildingCompleted.Invoke(this, new CompletedConstructionEventArgs(completedBuilding));
            }
        }

        private void Building_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Building_Destroyed;

			IBuilding destroyedBuilding = e.DestroyedTarget.Parse<IBuilding>();
            destroyedBuilding.CompletedBuildable -= Building_CompletedBuildable;

            if (BuildingDestroyed != null)
            {
				BuildingDestroyed.Invoke(this, new BuildingDestroyedEventArgs(destroyedBuilding));
            }
        }

        void Update()
        {
            RepairManager.Repair(Time.deltaTime);
        }
    }
}
