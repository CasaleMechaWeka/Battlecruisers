using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
	public abstract class Buildable : Target, IBuildable
	{
		private float _buildTimeInDroneSeconds;
		private float _buildProgressInDroneSeconds;

		protected UIManager _uiManager;
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

		#region Properties
		public BuildableState BuildableState { get; private set; }
		public virtual float Damage { get { return 0; } }
		public float BuildProgress { get; private set; }

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
		#endregion Properties

		public event EventHandler StartedConstruction;
		public event EventHandler CompletedBuildable;
		public event EventHandler<BuildProgressEventArgs> BuildableProgress;

		protected override void OnAwake()
		{
			_textMesh = gameObject.GetComponentInChildren<TextMesh>(includeInactive: true);
			Assert.IsNotNull(_textMesh);

			_buildableProgress = gameObject.GetComponentInChildren<BuildableProgressController>(includeInactive: true);
			Assert.IsNotNull(_buildableProgress);

			BuildableWrapper buildableWrapper = gameObject.GetComponentInInactiveParent<BuildableWrapper>();
			_healthBar = buildableWrapper.GetComponentInChildren<HealthBarController>(includeInactive: true);
			Assert.IsNotNull(_healthBar);

			_buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
			_buildProgressInDroneSeconds = 0;
			BuildableState = BuildableState.NotStarted;

			_healthBar.Initialise(this, followDamagable: true);
		}

		public virtual void Initialise(Faction faction, UIManager uiManager, ICruiser parentCruiser, 
			ICruiser enemyCruiser, IFactoryProvider factoryProvider, IAircraftProvider aircraftProvider)
		{
			_uiManager = uiManager;
			_parentCruiser = parentCruiser;
			_enemyCruiser = enemyCruiser;
			_droneManager = parentCruiser.DroneManager;
			_droneConsumerProvider = parentCruiser.DroneConsumerProvider;
			_aircraftProvider = aircraftProvider;

			_factoryProvider = factoryProvider;
			_prefabFactory = _factoryProvider.PrefabFactory;
			_targetsFactory = _factoryProvider.TargetsFactory;
			_angleCalculatorFactory = _factoryProvider.AngleCalculatorFactory;
			_movementControllerFactory = _factoryProvider.MovementControllerFactory;
			_targetPositionPredictorFactory = _factoryProvider.TargetPositionPredictorFactory;

			Faction = faction;
			BuildableState = BuildableState.NotStarted;

			OnInitialised();
		}

		protected virtual void OnInitialised() { }

		private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
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
			SetupDroneConsumer();

			EnableRenderers(false);
			BuildableState = BuildableState.InProgress;

			if (StartedConstruction != null)
			{
				StartedConstruction.Invoke(this, EventArgs.Empty);
			}
		}

		private void SetupDroneConsumer()
		{
			Assert.IsNull(DroneConsumer);
			DroneConsumer = _droneConsumerProvider.RequestDroneConsumer(numOfDronesRequired);
			_droneConsumerProvider.ActivateDroneConsumer(DroneConsumer);
		}

		void Update()
		{
			if (BuildableState == BuildableState.InProgress)
			{
				Assert.IsTrue(DroneConsumer.State != DroneConsumerState.Idle);
				_buildProgressInDroneSeconds += DroneConsumer.NumOfDrones * Time.deltaTime;

				if (BuildableProgress != null)
				{
					BuildProgress = _buildProgressInDroneSeconds / _buildTimeInDroneSeconds;
					if (BuildProgress > 1)
					{
						BuildProgress = 1;
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

		private void CleanUpDroneConsumer()
		{
			_droneConsumerProvider.ReleaseDroneConsumer(DroneConsumer);
			DroneConsumer = null;
		}

		public virtual void InitiateDelete() { }
	}
}
