using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    public class Cruiser : Target, ICruiser, IComparableItem
	{
		private IUIManager _uiManager;
        private ICruiser _enemyCruiser;
        private SpriteRenderer _renderer;
        private ICruiserHelper _helper;
        private SlotWrapperController _slotWrapperController;
        private IClickHandler _clickHandler;

        public int numOfDrones;
        public float yAdjustmentInM;
        public float highlightXAdjustmentInM;
        public string description;
        public string cruiserName;

        // ITarget
        public override TargetType TargetType { get { return TargetType.Cruiser; } }

        // IHighlightable
		public override Vector2 Size { get { return _renderer.bounds.size; } }
        public override float SizeMultiplier { get { return 0.25f; } }
        public override Vector2 PositionAdjustment
        {
            get
            {
                float xAdjustment = Direction == Direction.Right ? -highlightXAdjustmentInM : highlightXAdjustmentInM;
                return new Vector2(xAdjustment, 0);
            }
        }
        
		// IComparableItem
		public string Description { get { return description; } }
        public string Name { get { return cruiserName; } }
		public Sprite Sprite { get { return _renderer.sprite; } }

        // ICruiser
        public IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        public IDroneConsumerProvider DroneConsumerProvider { get; private set; }
        public Direction Direction { get; private set; }
        public float YAdjustmentInM { get { return yAdjustmentInM; } }
        public IFactoryProvider FactoryProvider { get; private set; }
        private FogOfWar _fog;
        public IFogOfWar Fog { get { return _fog; } }
		public IRepairManager RepairManager { get; private set; }
        public int NumOfDrones { get { return numOfDrones; } }
        public IBuildProgressCalculator BuildProgressCalculator { get; private set; }
        public bool IsPlayerCruiser { get { return Direction == Direction.Right; } }

        // ICruiserController
        public bool IsAlive { get { return !IsDestroyed; } }
        public ISlotWrapper SlotWrapper { get; private set; }
        public ISlotNumProvider SlotNumProvider { get; private set; }
        public IDroneManager DroneManager { get; private set; }

        public event EventHandler<StartedBuildingConstructionEventArgs> BuildingStarted;
        public event EventHandler<CompletedBuildingConstructionEventArgs> BuildingCompleted;
        public event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;
        public event EventHandler Clicked;

        protected override void OnStaticInitialised()
		{
            base.OnStaticInitialised();

			_renderer = GetComponent<SpriteRenderer>();
			Assert.IsNotNull(_renderer);

            _slotWrapperController = GetComponentInChildren<SlotWrapperController>(includeInactive: true);
            Assert.IsNotNull(_slotWrapperController);
            _slotWrapperController.StaticInitialise();
            SlotNumProvider = _slotWrapperController;

            _fog = GetComponentInChildren<FogOfWar>(includeInactive: true);
            Assert.IsNotNull(_fog);

            ClickHandlerWrapper clickHandlerWrapper = GetComponent<ClickHandlerWrapper>();
            Assert.IsNotNull(clickHandlerWrapper);
            _clickHandler = clickHandlerWrapper.GetClickHandler();
        }

        protected override ITextMesh GetRepairDroneNumText()
        {
            TextMesh numOfRepairDronesText = transform.FindNamedComponent<TextMesh>("NumOfDrones");
            Assert.IsNotNull(numOfRepairDronesText);
            return new TextMeshWrapper(numOfRepairDronesText);
        }

        public void Initialise(ICruiserArgs args)
        {
            Faction = args.Faction;
            _enemyCruiser = args.EnemyCruiser;
            _uiManager = args.UiManager;
            DroneManager = args.DroneManager;
            DroneManager.NumOfDrones = numOfDrones;
            DroneConsumerProvider = args.DroneConsumerProvider;
            FactoryProvider = args.FactoryProvider;
            Direction = args.FacingDirection;
            _helper = args.Helper;
            BuildProgressCalculator = args.BuildProgressCalculator;
			
            args.RepairManager.Initialise(this);
            RepairManager = args.RepairManager;

            _fog.Initialise(args.ShouldShowFog);

            SlotWrapper = _slotWrapperController.Initialise(this, args.HighlightableFilter);
            SlotWrapper.HideAllSlots();

            _clickHandler.SingleClick += _clickHandler_SingleClick;
            _clickHandler.DoubleClick += _clickHandler_DoubleClick;
		}

        private void _clickHandler_SingleClick(object sender, EventArgs e)
        {
            Logging.Log(Tags.CRUISER, "_clickHandler_SingleClick");

            _uiManager.ShowCruiserDetails(this);
            _helper.FocusCameraOnCruiser();

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }

        private void _clickHandler_DoubleClick(object sender, EventArgs e)
        {
            // FELIX  Create double click handler?  Have Player-/AI- handlers, will avoid ugly Faction check :/
            // Only allow double clicks to control player cruiser drones :P
            if (Faction == Faction.Blues
                && RepairCommand.CanExecute)
            {
                IDroneConsumer repairDroneConsumer = RepairManager.GetDroneConsumer(this);
                DroneManager.ToggleDroneConsumerFocus(repairDroneConsumer);
            }

            // Set as user chosen target, to make everything attack this building
            FactoryProvider.TargetsFactory.UserChosenTargetHelper.ToggleChosenTarget(this);
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

            _helper.ShowBuildingGroupButtons();

			building.StartConstruction();

			if (BuildingStarted != null)
			{
				BuildingStarted.Invoke(this, new StartedBuildingConstructionEventArgs(building));
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
                BuildingCompleted.Invoke(this, new CompletedBuildingConstructionEventArgs(completedBuilding));
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

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            SlotWrapper.DisposeManagedState();
        }
    }
}
