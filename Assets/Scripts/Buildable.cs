using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers
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
		private Renderer _renderer;
		private BuildableState _buildableState;
		private float _buildTimeInDroneSeconds;
		private float _buildProgressInDroneSeconds;

		protected UIManager _uiManager;
		protected Cruiser _parentCruiser;
		protected Cruiser _enemyCruiser;
		protected IDroneManager _droneManager;

		public string buildableName;
		public string description;
		public int numOfDronesRequired;
		public float buildTimeInS;
		public SlotType slotType;

		public TextMesh textMesh;
		public BuildableProgressController buildableProgress;

		public virtual float Damage { get { return 0; } }

		public virtual Vector3 Size 
		{ 
			get 
			{ 
				return _renderer.bounds.size; 
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

		public IDroneConsumer DroneConsumer { get; private set; }

		public event EventHandler Destroyed;
		public event EventHandler StartedBuilding;
		public event EventHandler PausedBuilding;
		public event EventHandler ResumedBuilding;
		public event EventHandler CompletedBuilding;
		public event EventHandler<BuildProgressEventArgs> BuildableProgress;

		void Awake()
		{
			Debug.Log("BuildableObject.Awake()");
			_renderer = GetComponent<Renderer>();

//			buildableProgress.image.sprite = Sprite;
			buildableProgress.image.rectTransform.sizeDelta = new Vector2(_renderer.bounds.size.x, _renderer.bounds.size.y);

			_buildTimeInDroneSeconds = numOfDronesRequired * buildTimeInS;
			_buildProgressInDroneSeconds = 0;
			_buildableState = BuildableState.NotStarted;
		}

		public virtual void Initialise(UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildableFactory buildableFactory, IDroneManager droneManager)
		{
			_buildableState = BuildableState.NotStarted;
			_uiManager = uiManager;
			_parentCruiser = parentCruiser;
			_enemyCruiser = enemyCruiser;
			_droneManager = droneManager;

			SetupDroneConsumer(numOfDronesRequired);
		}

		// For copying private members, and non-MonoBehaviour or primitive types (eg: ITurretStats).
		public virtual void Initialise(Buildable buildable)
		{
			_buildableState = BuildableState.NotStarted;
			_uiManager = buildable._uiManager;
			_parentCruiser = buildable._parentCruiser;
			_enemyCruiser = buildable._enemyCruiser;
			_droneManager = buildable._droneManager;

			SetupDroneConsumer(numOfDronesRequired);
		}

		private void SetupDroneConsumer(int numOfDronesRequired)
		{
			Assert.IsNull(DroneConsumer);
			DroneConsumer = new DroneConsumer(numOfDronesRequired);
			DroneConsumer.DroneNumChanged += DroneConsumer_DroneNumChanged;
			DroneConsumer.DroneStateChanged += DroneConsumer_DroneStateChanged;
		}

		private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
		{
			textMesh.text = e.NewNumOfDrones.ToString();
		}

		private void DroneConsumer_DroneStateChanged(object sender, DroneStateChangedEventArgs e)
		{
			if (e.OldState == DroneConsumerState.Idle)
			{
				_buildableState = BuildableState.InProgress;
			}
			else if (e.NewState == DroneConsumerState.Idle)
			{
				_buildableState = BuildableState.Paused;
			}
		}

		public void StartBuilding()
		{
			_droneManager.AddDroneConsumer(DroneConsumer);

			_renderer.enabled = false;
			_buildableState = BuildableState.InProgress;

			if (StartedBuilding != null)
			{
				StartedBuilding.Invoke(this, EventArgs.Empty);
			}
		}

		void Update()
		{
			if (_buildableState == BuildableState.InProgress)
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
					OnBuildingCompleted();
				}
			}

			OnUpdate();
		}

		protected virtual void OnUpdate() { }

		protected virtual void OnBuildingCompleted()
		{
			_droneManager.RemoveDroneConsumer(DroneConsumer);

			_renderer.enabled = true;
			_buildableState = BuildableState.Completed;

			DroneConsumer.DroneNumChanged -= DroneConsumer_DroneNumChanged;
			DroneConsumer.DroneStateChanged -= DroneConsumer_DroneStateChanged;
			DroneConsumer = null;
			
			if (CompletedBuilding != null)
			{
				CompletedBuilding.Invoke(this, EventArgs.Empty);
			}
		}
		
		void OnDestroy()
		{
			Debug.Log("Buildable.OnDestroy()");

			if (_buildableState == BuildableState.InProgress || _buildableState == BuildableState.Paused)
			{
				_droneManager.RemoveDroneConsumer(DroneConsumer);
			}

			OnDestroyed();

			if (Destroyed != null)
			{
				Destroyed.Invoke(this, EventArgs.Empty);
			}
		}

		protected virtual void OnDestroyed() { }

		public virtual void InitiateDelete() { }
	}
}
