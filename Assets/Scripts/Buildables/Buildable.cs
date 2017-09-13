using System;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Targets;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public abstract class Buildable : Target, IBuildable
	{
		private float _buildProgressInDroneSeconds;
		private float _buildTimeInDroneSeconds;

		protected IUIManager _uiManager;
		protected ICruiser _parentCruiser;
		protected ICruiser _enemyCruiser;
		protected IPrefabFactory _prefabFactory;
		protected IDroneManager _droneManager;
		protected IDroneConsumerProvider _droneConsumerProvider;
		protected ITargetsFactory _targetsFactory;
		protected IMovementControllerFactory _movementControllerFactory;
		protected IAircraftProvider _aircraftProvider;
		protected IAngleCalculatorFactory _angleCalculatorFactory;
		protected ITargetPositionPredictorFactory _targetPositionPredictorFactory;
		protected IFactoryProvider _factoryProvider;

		public string buildableName;
		public string description;
		public int numOfDronesRequired;
		public float buildTimeInS;
		public SlotType slotType;

		private TextMesh _textMesh;
		protected BuildableProgressController _buildableProgress;
		private HealthBarController _healthBar;

        private const float MAX_BUILD_PROGRESS = 1;
        private const float DEFAULT_BOOST_MULTIPLIER = 1;
        // FELIX  TEMP
        private const float BUILD_CHEAT_MULTIPLIER = 50;

		#region Properties
		public BuildableState BuildableState { get; private set; }
		public virtual float Damage { get { return 0; } }
		public float BuildProgress { get; private set; }
        public string Name { get { return buildableName; } }
        public int NumOfDronesRequired { get { return numOfDronesRequired; } }
        public float BuildTimeInS { get { return buildTimeInS; } }

		Vector2 IBuildable.Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        Quaternion IBuildable.Rotation
        {
            set { transform.rotation = value; }
        }

		public virtual Vector3 Size 
		{ 
			get 
			{ 
				return Renderer.bounds.size; 
			} 
		}

		protected Renderer _renderer;
		protected virtual Renderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					_renderer = GetComponent<Renderer>();
				}
				return _renderer;
			}
		}

		protected Sprite _sprite;
		public virtual Sprite Sprite
		{
			get
			{
				if (_sprite == null)
				{
					_sprite = GetComponent<SpriteRenderer>().sprite;
				}
				return _sprite;
			}
		}

		private IDroneConsumer _droneConsumer;
		public IDroneConsumer DroneConsumer 
		{ 
			get { return _droneConsumer; }
			protected set
			{
				if (_droneConsumer != null)
				{
					_droneConsumer.DroneNumChanged -= DroneConsumer_DroneNumChanged;
					_droneConsumer.DroneStateChanged -= DroneConsumer_DroneStateChanged;
				}

				_droneConsumer = value;

				if (_droneConsumer != null)
				{
					_droneConsumer.DroneNumChanged += DroneConsumer_DroneNumChanged;
					_droneConsumer.DroneStateChanged += DroneConsumer_DroneStateChanged;
				}
			}
		}

        public string Description { get { return description; } }

        public SlotType SlotType { get { return slotType; } }

        protected abstract HealthBarController HealthBarController { get; }

        private float _healthGainperDroneS;
        public override float HealthGainPerDroneS { get { return _healthGainperDroneS; } }

        public float BoostMultiplier { set; private get; }
        #endregion Properties

        public event EventHandler StartedConstruction;
		public event EventHandler CompletedBuildable;
		public event EventHandler<BuildProgressEventArgs> BuildableProgress;

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			_textMesh = gameObject.GetComponentInChildren<TextMesh>(includeInactive: true);
			Assert.IsNotNull(_textMesh);

			_buildableProgress = gameObject.GetComponentInChildren<BuildableProgressController>(includeInactive: true);
			Assert.IsNotNull(_buildableProgress);
			_buildableProgress.Initialise();

            _healthBar = HealthBarController;
			Assert.IsNotNull(_healthBar);
			_healthBar.Initialise(this, followDamagable: true);

            BoostMultiplier = DEFAULT_BOOST_MULTIPLIER;
		}

		protected void Initialise(ICruiser parentCruiser, ICruiser enemyCruiser, IUIManager uiManager, IFactoryProvider factoryProvider)
		{
            Assert.IsNotNull(_textMesh, "Must call StaticInitialise() before Initialise(...)");

			_parentCruiser = parentCruiser;
			_enemyCruiser = enemyCruiser;
			_droneManager = _parentCruiser.DroneManager;
			_droneConsumerProvider = _parentCruiser.DroneConsumerProvider;
			_uiManager = uiManager;
			_aircraftProvider = factoryProvider.AircraftProvider;

			_factoryProvider = factoryProvider;
			_prefabFactory = _factoryProvider.PrefabFactory;
			_targetsFactory = _factoryProvider.TargetsFactory;
			_angleCalculatorFactory = _factoryProvider.AngleCalculatorFactory;
			_movementControllerFactory = _factoryProvider.MovementControllerFactory;
			_targetPositionPredictorFactory = _factoryProvider.TargetPositionPredictorFactory;

			Faction = _parentCruiser.Faction;
			BuildableState = BuildableState.NotStarted;
			_buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
			_buildProgressInDroneSeconds = 0;
            _healthGainperDroneS = _buildTimeInDroneSeconds / maxHealth;
		}

		protected virtual void OnInitialised() { }

		protected virtual void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
		{
			_textMesh.text = e.NewNumOfDrones.ToString();
		}

		private void DroneConsumer_DroneStateChanged(object sender, DroneStateChangedEventArgs e)
		{
			if (BuildableState != BuildableState.Completed)
			{
				if (e.OldState == DroneConsumerState.Idle)
				{
					BuildableState = BuildableState.InProgress;
				}
				else if (e.NewState == DroneConsumerState.Idle)
				{
					BuildableState = BuildableState.Paused;
				}
			}
		}

		public void StartConstruction()
		{
            SetupDroneConsumer(numOfDronesRequired);

			EnableRenderers(false);

            if (DroneConsumer.State != DroneConsumerState.Idle)
            {
                BuildableState = BuildableState.InProgress;
			}

			if (StartedConstruction != null)
			{
				StartedConstruction.Invoke(this, EventArgs.Empty);
			}
		}

		void Update()
		{
			if (BuildableState == BuildableState.InProgress)
			{
				Assert.IsTrue(DroneConsumer.State != DroneConsumerState.Idle);
                _buildProgressInDroneSeconds += DroneConsumer.NumOfDrones * BoostMultiplier * Time.deltaTime * BUILD_CHEAT_MULTIPLIER;

				if (BuildableProgress != null)
				{
					BuildProgress = _buildProgressInDroneSeconds / _buildTimeInDroneSeconds;
                    if (BuildProgress > MAX_BUILD_PROGRESS)
					{
                        BuildProgress = MAX_BUILD_PROGRESS;
					}

					BuildableProgress.Invoke(this, new BuildProgressEventArgs(this));
				}

				if (_buildProgressInDroneSeconds >= _buildTimeInDroneSeconds)
				{
					OnBuildableCompleted();
				}
			}

			OnUpdate();
		}

		protected virtual void OnUpdate() { }

		protected virtual void OnBuildableCompleted()
		{
			CleanUpDroneConsumer();

			EnableRenderers(true);
			BuildableState = BuildableState.Completed;

			if (CompletedBuildable != null)
			{
				CompletedBuildable.Invoke(this, EventArgs.Empty);
			}

            RepairCommand.EmitCanExecuteChanged();
		}

		protected virtual void EnableRenderers(bool enabled)
		{
			Renderer.enabled = enabled;
		}

		// All buildables are wrapped by a UnitWrapper or BuildingWrapper, which contains
		// both the target and the health bar.  Hence destroy wrapper, so health bar
		// gets destroyed at the same time as the target.
		protected override void InternalDestroy()
		{
			Assert.IsNotNull(transform.parent);
			Destroy(transform.parent.gameObject);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			if (DroneConsumer != null)
			{
				CleanUpDroneConsumer();
			}
		}

		protected void SetupDroneConsumer(int numOfDrones)
		{
			Assert.IsNull(DroneConsumer);
            DroneConsumer = _droneConsumerProvider.RequestDroneConsumer(numOfDrones, isHighPriority: true);
			_droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
		}


		protected void CleanUpDroneConsumer()
		{
            Assert.IsNotNull(DroneConsumer);
			_droneConsumerProvider.ReleaseDroneConsumer(DroneConsumer);
			DroneConsumer = null;
		}

        protected override bool CanRepairCommandExecute()
        {
            return
                base.CanRepairCommandExecute()
                && BuildableState == BuildableState.Completed;       
        }

		public virtual void InitiateDelete() { }
	}
}
