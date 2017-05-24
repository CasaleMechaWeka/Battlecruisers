using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Units.Aircraft.Providers;
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
		protected IAircraftProvider _aircraftProvider;

		public string buildableName;
		public string description;
		public int numOfDronesRequired;
		public float buildTimeInS;
		public SlotType slotType;

		public TextMesh textMesh;
		public BuildableProgressController buildableProgress;
		public HealthBarController healthBar;

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
			_buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
			_buildProgressInDroneSeconds = 0;
			BuildableState = BuildableState.NotStarted;

			healthBar.Initialise(this, followDamagable: true);
		}

		public virtual void Initialise(Faction faction, UIManager uiManager, ICruiser parentCruiser, 
			ICruiser enemyCruiser, IPrefabFactory prefabFactory, ITargetsFactory targetsFactory, IAircraftProvider aircraftProvider)
		{
			_uiManager = uiManager;
			_parentCruiser = parentCruiser;
			_enemyCruiser = enemyCruiser;
			_prefabFactory = prefabFactory;
			_droneManager = parentCruiser.DroneManager;
			_droneConsumerProvider = parentCruiser.DroneConsumerProvider;
			_targetsFactory = targetsFactory;
			_aircraftProvider = aircraftProvider;

			Faction = faction;
			BuildableState = BuildableState.NotStarted;

			OnInitialised();
		}

		protected virtual void OnInitialised() { }

		private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
		{
			textMesh.text = e.NewNumOfDrones.ToString();
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
