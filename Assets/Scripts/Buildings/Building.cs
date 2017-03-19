using BattleCruisers.Cruisers;
using BattleCruisers.Buildings.Turrets;
using BattleCruisers.UI;
using BattleCruisers.Units;
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

	public class Building : BuildableObject
	{
		protected UIManager _uiManager;
		protected Cruiser _parentCruiser;

		public BuildingCategory category;
		public SlotType slotType;
		// Proportional to building size
		public float customOffsetProportion;

		public Action OnDestroyed;

		public virtual void Initialise(UIManager uiManagerArg, Cruiser parentCruiser, Cruiser enemyCruiser, BuildingFactory buildingFactory)
		{
			_uiManager = uiManagerArg;
			_parentCruiser = parentCruiser;
			faction = _parentCruiser.faction;
		}

		// For copying private members, and non-MonoBehaviour or primitive types (eg: ITurretStats).
		public virtual void Initialise(Building building)
		{
			_uiManager = building._uiManager;
			_parentCruiser = building._parentCruiser;
		}

		void OnMouseDown()
		{
			_uiManager.SelectBuilding(this, _parentCruiser);
			OnClicked();
		}

		protected virtual void OnClicked() { }

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
