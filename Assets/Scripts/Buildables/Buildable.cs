using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
	public enum BuildableState
	{
		NotStarted, InProgress, Paused, Completed
	}

	public class BuildProgressEventArgs : EventArgs
	{
		public float BuildProgress { get; private set; }

		public BuildProgressEventArgs(float buildProgress)
		{
			BuildProgress = buildProgress;
		}
	}

	// FELIX  Create interface
	public abstract class Buildable : FactionObject
	{
		private float _buildTimeInDroneSeconds;
		private float _buildProgressInDroneSeconds;

		protected UIManager _uiManager;
		protected Cruiser _parentCruiser;
		protected Cruiser _enemyCruiser;
		protected BuildableFactory _buildableFactory;
		protected IDroneManager _droneManager;
		protected IDroneConsumerProvider _droneConsumerProvider;

		public string buildableName;
		public string description;
		public int numOfDronesRequired;
		public float buildTimeInS;
		public SlotType slotType;

		public TextMesh textMesh;
		public BuildableProgressController buildableProgress;

		public BuildableState BuildableState { get; private set; }
		public virtual float Damage { get { return 0; } }

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

		public event EventHandler Destroyed;
		public event EventHandler StartedConstruction;
		public event EventHandler PausedBuilding;
		public event EventHandler ResumedBuilding;
		public event EventHandler CompletedBuildable;
		public event EventHandler<BuildProgressEventArgs> BuildableProgress;

		public void Awake()
		{
			Debug.Log("BuildableObject.Awake()");

			buildableProgress.image.rectTransform.sizeDelta = new Vector2(Size.x, Size.y);

			_buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
			_buildProgressInDroneSeconds = 0;
			BuildableState = BuildableState.NotStarted;

			OnAwake();
		}

		protected virtual void OnAwake() { }

		// FELIX  DroneManager & BuildableFactory not used by most buildings, find different way of injecting?
		public virtual void Initialise(Faction faction, UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildableFactory buildableFactory)
		{
			Faction = faction;
			BuildableState = BuildableState.NotStarted;
			_uiManager = uiManager;
			_parentCruiser = parentCruiser;
			_enemyCruiser = enemyCruiser;
			_buildableFactory = buildableFactory;
			_droneManager = parentCruiser.DroneManager;
			_droneConsumerProvider = parentCruiser.DroneConsumerProvider;
		}

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
					float buildProgress = _buildProgressInDroneSeconds / _buildTimeInDroneSeconds;
					BuildableProgress.Invoke(this, new BuildProgressEventArgs(buildProgress));
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
		
		void OnDestroy()
		{
			Debug.Log("Buildable.OnDestroy()");

			if (DroneConsumer != null)
			{
				CleanUpDroneConsumer();
			}

			OnDestroyed();

			if (Destroyed != null)
			{
				Destroyed.Invoke(this, EventArgs.Empty);
			}
		}

		private void CleanUpDroneConsumer()
		{
			_droneConsumerProvider.ReleaseDroneConsumer(DroneConsumer);
			DroneConsumer = null;
		}

		protected virtual void OnDestroyed() { }

		public virtual void InitiateDelete() { }
	}
}
