using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects.Smoke;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions;
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
        private Rigidbody2D _rigidBody;
        private ICruiserHelper _helper;
        private SlotWrapperController _slotWrapperController;
        private IClickHandler _clickHandler;
        private IDoubleClickHandler<IBuilding> _buildingDoubleClickHandler;
        private IDoubleClickHandler<ICruiser> _cruiserDoubleClickHandler;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private IManagedDisposable _fogOfWarManager, _unitReadySignal, _droneFeedbackSound;
#pragma warning restore CS0414  // Variable is assigned but never used
        private const float ON_DEATH_GRAVITY_SCALE = 0.01f;

        public int numOfDrones;
        public float yAdjustmentInM;
        public string description;
        public string cruiserName;

        // ITarget
        public override TargetType TargetType => TargetType.Cruiser;
        public override Color Color { set { _renderer.color = value; } }
        public override Vector2 Size => _renderer.bounds.size;
        
		// IComparableItem
		public string Description => description;
        public string Name => cruiserName;
		public Sprite Sprite => _renderer.sprite;

        // ICruiser
        public IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        public IDroneConsumerProvider DroneConsumerProvider { get; private set; }
        public Direction Direction { get; private set; }
        public float YAdjustmentInM => yAdjustmentInM;
        public IFactoryProvider FactoryProvider { get; private set; }
        public ICruiserSpecificFactories CruiserSpecificFactories { get; private set; }
        private FogOfWar _fog;
        public IGameObject Fog => _fog;
		public IRepairManager RepairManager { get; private set; }
        public int NumOfDrones => numOfDrones;
        public IBuildProgressCalculator BuildProgressCalculator { get; private set; }
        public bool IsPlayerCruiser => Position.x < 0;

        // ICruiserController
        public bool IsAlive => !IsDestroyed;
        public ISlotAccessor SlotAccessor { get; private set; }
        public ISlotHighlighter SlotHighlighter { get; private set; }
        public ISlotNumProvider SlotNumProvider { get; private set; }
        public IDroneManager DroneManager { get; private set; }
        public IDroneFocuser DroneFocuser { get; private set; }
        public ICruiserBuildingMonitor BuildingMonitor { get; private set; }
        public ICruiserUnitMonitor UnitMonitor { get; private set; }
        public IPopulationLimitMonitor PopulationLimitMonitor { get; private set; }
        public IUnitTargets UnitTargets { get; private set; }
        public ITargetTracker BlockedShipsTracker { get; private set; }

        public event EventHandler<BuildingStartedEventArgs> BuildingStarted;
        public event EventHandler<BuildingCompletedEventArgs> BuildingCompleted;
        public event EventHandler<BuildingDestroyedEventArgs> BuildingDestroyed;
        public event EventHandler Clicked;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

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

            _rigidBody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(_rigidBody);

            BuildingMonitor = new CruiserBuildingMonitor(this);
            UnitMonitor = new CruiserUnitMonitor(BuildingMonitor);
            PopulationLimitMonitor = new PopulationLimitMonitor(UnitMonitor);
            UnitTargets = new UnitTargets(UnitMonitor);
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
            CruiserSpecificFactories = args.CruiserSpecificFactories;
            Direction = args.FacingDirection;
            _helper = args.Helper;
            BuildProgressCalculator = args.BuildProgressCalculator;
            _buildingDoubleClickHandler = args.BuildingDoubleClickHandler;
            _cruiserDoubleClickHandler = args.CruiserDoubleClickHandler;
            _fogOfWarManager = args.FogOfWarManager;
            RepairManager = args.RepairManager;

            _fog.Initialise(args.FogStrength);

            SlotAccessor = _slotWrapperController.Initialise(this);
            SlotHighlighter = new SlotHighlighter(SlotAccessor, args.HighlightableFilter, BuildingMonitor);

            EnemyShipBlockerInitialiser enemyShipBlockerInitialiser = GetComponentInChildren<EnemyShipBlockerInitialiser>();
            Assert.IsNotNull(enemyShipBlockerInitialiser);
            BlockedShipsTracker
                = enemyShipBlockerInitialiser.Initialise(
                    args.FactoryProvider.Targets,
                    args.CruiserSpecificFactories.Targets.TrackerFactory,
                    args.EnemyCruiser.Faction);

            UnitReadySignalInitialiser unitReadySignalInitialiser = GetComponentInChildren<UnitReadySignalInitialiser>();
            Assert.IsNotNull(unitReadySignalInitialiser);
            _unitReadySignal = unitReadySignalInitialiser.CreateSignal(this);

            DroneSoundFeedbackInitialiser droneSoundFeedbackInitialiser = GetComponentInChildren<DroneSoundFeedbackInitialiser>();
            Assert.IsNotNull(droneSoundFeedbackInitialiser);
            _droneFeedbackSound = droneSoundFeedbackInitialiser.Initialise(args.HasActiveDrones);

            _clickHandler.SingleClick += _clickHandler_SingleClick;
            _clickHandler.DoubleClick += _clickHandler_DoubleClick;
		}

        private void _clickHandler_SingleClick(object sender, EventArgs e)
        {
            Logging.LogMethod(Tags.CRUISER);

            _uiManager.ShowCruiserDetails(this);
            _helper.FocusCameraOnCruiser();

            Clicked?.Invoke(this, EventArgs.Empty);
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

            IBuilding building = FactoryProvider.PrefabFactory.CreateBuilding(SelectedBuildingPrefab, _uiManager, FactoryProvider);

            building.Activate(
                new BuildingActivationArgs(
                    this,
                    _enemyCruiser,
                    CruiserSpecificFactories,
                    slot,
                    _buildingDoubleClickHandler));

            slot.SetBuilding(building);

			building.CompletedBuildable += Building_CompletedBuildable;
            building.Destroyed += Building_Destroyed;

            _helper.ShowBuildingGroupButtons();

			building.StartConstruction();

			BuildingStarted?.Invoke(this, new BuildingStartedEventArgs(building));

			return building;
		}

        private void Building_CompletedBuildable(object sender, EventArgs e)
        {
            IBuilding completedBuilding = sender.Parse<IBuilding>();
            completedBuilding.CompletedBuildable -= Building_CompletedBuildable;

            BuildingCompleted?.Invoke(this, new BuildingCompletedEventArgs(completedBuilding));
        }

        private void Building_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= Building_Destroyed;

			IBuilding destroyedBuilding = e.DestroyedTarget.Parse<IBuilding>();
            destroyedBuilding.CompletedBuildable -= Building_CompletedBuildable;

            BuildingDestroyed?.Invoke(this, new BuildingDestroyedEventArgs(destroyedBuilding));
        }

        void Update()
        {
            RepairManager.Repair(_time.DeltaTime);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            FactoryProvider.Sound.SoundPlayer.PlaySoundAsync(SoundKeys.Deaths.Cruiser, Position);

            // Make cruiser sink
            _rigidBody.bodyType = RigidbodyType2D.Dynamic;
            _rigidBody.gravityScale = ON_DEATH_GRAVITY_SCALE;

            // Make cruiser rear sink first
            _rigidBody.AddTorque(0.75f, ForceMode2D.Impulse);
        }

        protected override void InternalDestroy()
        {
            // Do not destroy game object, to give time for cruiser to sink
        }

        public void MakeInvincible()
        {
            _healthTracker.State = HealthTrackerState.Immutable;
        }
    }
}
