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
	// FELIX  Create interface
	public abstract class Buildable : FactionObject
	{
		private Renderer _renderer;

		protected UIManager _uiManager;
		protected Cruiser _parentCruiser;
		protected Cruiser _enemyCruiser;

		public string buildableName;
		public string description;
		public int numOfDronesRequired;
		public float buildTimeInS;
		public SlotType slotType;

		public TextMesh textMesh;

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

		public virtual void InitiateDelete() { }

		public virtual void Initialise(UIManager uiManager, Cruiser parentCruiser, Cruiser enemyCruiser, BuildableFactory buildableFactory)
		{
			_uiManager = uiManager;
			_parentCruiser = parentCruiser;
			_enemyCruiser = enemyCruiser;
		}

		// For copying private members, and non-MonoBehaviour or primitive types (eg: ITurretStats).
		public virtual void Initialise(Buildable buildable)
		{
			_uiManager = buildable._uiManager;
			_parentCruiser = buildable._parentCruiser;
			_enemyCruiser = buildable._enemyCruiser;
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

		void Awake()
		{
			Debug.Log("BuildableObject.Awake()");
			_renderer = GetComponent<Renderer>();
		}

		public void StartBuilding()
		{
			SetupDroneConsumer(numOfDronesRequired);

			if (StartedBuilding != null)
			{
				StartedBuilding.Invoke(this, EventArgs.Empty);
			}
		}

		void OnDestroy()
		{
			Debug.Log("Buildable.OnDestroy()");

			// The original prefab will not have called Initalise(), so will not have DroneConsumer set.
			if (DroneConsumer != null)
			{
				DroneConsumer.DroneStateChanged -= OnDroneStateChanged;
			}

			if (Destroyed != null)
			{
				Destroyed.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
