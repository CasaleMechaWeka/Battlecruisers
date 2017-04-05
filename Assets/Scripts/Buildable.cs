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
		NotStarted, Building, Completed
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

		protected UIManager _uiManager;
		protected Cruiser _parentCruiser;
		protected Cruiser _enemyCruiser;

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
		public event EventHandler<BuildProgressEventArgs> BuildingProgress;

		void Awake()
		{
			Debug.Log("BuildableObject.Awake()");
			_renderer = GetComponent<Renderer>();

			buildableProgress.image.sprite = Sprite;
		}

		// FELIX  Avoid last 2 parameters?  Only used by some buildings...
		public virtual void Initialise(UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildableFactory buildableFactory, IDroneManager droneManager)
		{
			_buildableState = BuildableState.NotStarted;
			_uiManager = uiManager;
			_parentCruiser = parentCruiser;
			_enemyCruiser = enemyCruiser;

			SetupDroneConsumer(numOfDronesRequired);
		}

		// For copying private members, and non-MonoBehaviour or primitive types (eg: ITurretStats).
		public virtual void Initialise(Buildable buildable)
		{
			_buildableState = BuildableState.NotStarted;
			_uiManager = buildable._uiManager;
			_parentCruiser = buildable._parentCruiser;
			_enemyCruiser = buildable._enemyCruiser;

			SetupDroneConsumer(numOfDronesRequired);
		}

		private void SetupDroneConsumer(int numOfDronesRequired)
		{
			Assert.IsNull(DroneConsumer);
			DroneConsumer = new DroneConsumer(numOfDronesRequired);
			DroneConsumer.DroneNumChanged += DroneConsumer_DroneNumChanged;
			DroneConsumer.DroneStateChanged += OnDroneStateChanged;
		}

		private void DroneConsumer_DroneNumChanged(object sender, DroneNumChangedEventArgs e)
		{
			textMesh.text = e.NewNumOfDrones.ToString();
		}

		private void OnDroneStateChanged(object sender, DroneStateChangedEventArgs e)
		{

		}

		public void StartBuilding()
		{
			_buildableState = BuildableState.Building;
			buildableProgress.gameObject.SetActive(true);

			if (StartedBuilding != null)
			{
				StartedBuilding.Invoke(this, EventArgs.Empty);
			}

			// FELIX  TEMP
			Invoke("Temp", 3);
		}

		private void Temp()
		{
			OnBuildingCompleted();
		}

		protected virtual void OnBuildingCompleted()
		{
			_buildableState = BuildableState.Completed;
			buildableProgress.gameObject.SetActive(false);

			if (CompletedBuilding != null)
			{
				CompletedBuilding.Invoke(this, EventArgs.Empty);
			}
		}

		void OnDestroy()
		{
			Debug.Log("Buildable.OnDestroy()");

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
