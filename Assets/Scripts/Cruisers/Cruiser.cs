using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Smoke;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions;
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
        private IDoubleClickHandler<IBuilding> _buildingDoubleClickHandler;
        private IDoubleClickHandler<ICruiser> _cruiserDoubleClickHandler;
        private IUnitConstructionMonitor _unitConstructionMonitor;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private FogOfWarManager _fogOfWarManager;
        private SmokeGroupInitialiser _smokeGroup;
#pragma warning restore CS0414  // Variable is assigned but never used

        public int numOfDrones;
        public float yAdjustmentInM;
        public string description;
        public string cruiserName;

        // ITarget
        public override TargetType TargetType { get { return TargetType.Cruiser; } }
        public override Color Color { set { _renderer.color = value; } }
        public override Vector2 Size { get { return _renderer.bounds.size; } }
        
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
        public IGameObject Fog { get { return _fog; } }
		public IRepairManager RepairManager { get; private set; }
        public int NumOfDrones { get { return numOfDrones; } }
        public IBuildProgressCalculator BuildProgressCalculator { get; private set; }
        public bool IsPlayerCruiser { get { return Position.x < 0; } }

        // ICruiserController
        public bool IsAlive { get { return !IsDestroyed; } }
        public ISlotAccessor SlotAccessor { get; private set; }
        public ISlotHighlighter SlotHighlighter { get; private set; }
        public ISlotNumProvider SlotNumProvider { get; private set; }
        public IDroneManager DroneManager { get; private set; }
        public IDroneFocuser DroneFocuser { get; private set; }

        public event EventHandler<StartedBuildingConstructionEventArgs> BuildingStarted;
        public event EventHandler<CompletedBuildingConstructionEventArgs> BuildingCompleted;
        public event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;
        public event EventHandler Clicked;

        public event EventHandler<StartedUnitConstructionEventArgs> StartedBuildingUnit
        {
            add { _unitConstructionMonitor.StartedBuildingUnit += value; }
            remove { _unitConstructionMonitor.StartedBuildingUnit -= value; }
        }
        public event EventHandler<CompletedUnitConstructionEventArgs> CompletedBuildingUnit
        {
            add { _unitConstructionMonitor.CompletedBuildingUnit += value; }
            remove { _unitConstructionMonitor.CompletedBuildingUnit -= value; }
        }

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

            _smokeGroup = GetComponentInChildren<SmokeGroupInitialiser>(includeInactive: true);
            Assert.IsNotNull(_smokeGroup);
        }

        protected override ITextMesh GetRepairDroneNumText()
        {
            TextMesh numOfRepairDronesText = transform.FindNamedComponent<TextMesh>("NumOfDrones");
            Assert.IsNotNull(numOfRepairDronesText);
            return new TextMeshWrapper(numOfRepairDronesText);
        }

        public virtual void Initialise(ICruiserArgs args)
        {
            Faction = args.Faction;
            _enemyCruiser = args.EnemyCruiser;
            _uiManager = args.UiManager;
            DroneManager = args.DroneManager;
            DroneFocuser = args.DroneFocuser;
            DroneManager.NumOfDrones = numOfDrones;
            DroneConsumerProvider = args.DroneConsumerProvider;
            FactoryProvider = args.FactoryProvider;
            Direction = args.FacingDirection;
            _helper = args.Helper;
            BuildProgressCalculator = args.BuildProgressCalculator;
            _buildingDoubleClickHandler = args.BuildingDoubleClickHandler;
            _cruiserDoubleClickHandler = args.CruiserDoubleClickHandler;
            _fogOfWarManager = args.FogOfWarManager;

            args.RepairManager.Initialise(this);
            RepairManager = args.RepairManager;

            _fog.Initialise(args.FogStrength);

            SlotAccessor = _slotWrapperController.Initialise(this);
            SlotHighlighter = new SlotHighlighter(SlotAccessor, args.HighlightableFilter, this);

            _unitConstructionMonitor = new UnitConstructionMonitor(this);

            _smokeGroup.Initialise(this, showSmokeWhenDestroyed: true);

            _clickHandler.SingleClick += _clickHandler_SingleClick;
            _clickHandler.DoubleClick += _clickHandler_DoubleClick;
		}

        private void _clickHandler_SingleClick(object sender, EventArgs e)
        {
            Logging.LogDefault(Tags.CRUISER);

            _uiManager.ShowCruiserDetails(this);
            _helper.FocusCameraOnCruiser();

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }

        private void _clickHandler_DoubleClick(object sender, EventArgs e)
        {
            _cruiserDoubleClickHandler.OnDoubleClick(this);
        }

        public IBuilding ConstructBuilding(IBuildableWrapper<IBuilding> buildingPrefab, ISlot slot)
        {
            Logging.Log(Tags.CRUISER, buildingPrefab.Buildable.Name);

			SelectedBuildingPrefab = buildingPrefab;
			return ConstructSelectedBuilding(slot);
		}

        public IBuilding ConstructSelectedBuilding(ISlot slot)
		{
			Assert.IsNotNull(SelectedBuildingPrefab);
            Assert.AreEqual(SelectedBuildingPrefab.Buildable.SlotSpecification.SlotType, slot.Type);

            IBuilding building = FactoryProvider.PrefabFactory.CreateBuilding(SelectedBuildingPrefab);
            building.Initialise(this, _enemyCruiser, _uiManager, FactoryProvider, slot, _buildingDoubleClickHandler);
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
            FactoryProvider.Sound.SoundPlayer.PlaySound(SoundKeys.Deaths.Cruiser, Position);
        }

        public void MakeInvincible()
        {
            _healthTracker.State = HealthTrackerState.Immutable;
        }
    }
}
