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
		private UIManager _uiManager;
		private Cruiser _parentCruiser;

		public string buildingName;
		public string description;
		public int numOfDronesRequired;
		public int buildTimeInS;
		public BuildingCategory category;
		public SlotType slotType;
		public float health;
		// Proportional to building size
		public float customOffsetProportion;

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
		
		public virtual void Initialise(UIManager uiManagerArg, Cruiser parentCruiser, Cruiser enemyCruiser, BuildingFactory buildingFactory)
		{
			_uiManager = uiManagerArg;
			_parentCruiser = parentCruiser;
		}

		// For copying private members, and non-MonoBehaviour or primitive types (eg: ITurretStats).
		public virtual void Initialise(Building building)
		{
			_uiManager = building._uiManager;
			_parentCruiser = building._parentCruiser;
		}

		void Awake()
		{
			Debug.Log("Building.Awake()");
			_renderer = GetComponent<Renderer>();
		}

		void OnMouseDown()
		{
			_uiManager.SelectBuilding(this, _parentCruiser);
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
