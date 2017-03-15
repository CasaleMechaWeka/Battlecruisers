using BattleCruisers.Cruisers;
using BattleCruisers.Buildings.Turrets;
using BattleCruisers.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.Buildings
{
	public enum BuildingCategory
	{
		Factory, Defence, Offence, Tactical, Support, Ultras
	}

	public class Building : MonoBehaviour
	{
		private Renderer _renderer;

		public string buildingName;
		public string description;
		public int numOfDronesRequired;
		public int buildTimeInS;
		public BuildingCategory category;
		public SlotType slotType;
		public float health;

		public UIManager UIManager { private get; set; }
		public Cruiser ParentCruiser { protected get; set; }

		public Action OnDestroyed;

		public virtual Vector3 Size 
		{ 
			get 
			{ 
				return _renderer.bounds.size; 
			} 
		}

		protected Sprite _buidlingSprite;
		public virtual Sprite BuildingSprite
		{
			get
			{
				if (_buidlingSprite == null)
				{
					_buidlingSprite = GetComponent<SpriteRenderer>().sprite;
				}
				return _buidlingSprite;
			}
		}

		void Awake()
		{
			Debug.Log("Building.Awake()");
			_renderer = GetComponent<Renderer>();
		}

		void OnMouseDown()
		{
			// FELIX  Differentiate between friendly and enemy cruiser
			UIManager.SelectBuildingFromFriendlyCruiser(this);
		}

		void OnDestroy()
		{
			Debug.Log("Building.OnDestroy()");
			if (OnDestroyed != null)
			{
				OnDestroyed.Invoke();
				OnDestroyed = null;
			}
		}

		public void InitiateDelete()
		{
			Destroy(gameObject);
		}

		public void CancelDelete()
		{
			throw new NotImplementedException();
		}
	}
}
