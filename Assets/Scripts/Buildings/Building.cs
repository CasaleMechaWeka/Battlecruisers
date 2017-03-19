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
		public BuildingCategory category;
		// Proportional to building size
		public float customOffsetProportion;

		// FELIX  Used?  Create event so can have multiple listeners
		public Action OnDestroyed;

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

		public override void InitiateDelete()
		{
			Destroy(gameObject);
		}

		public void CancelDelete()
		{
			throw new NotImplementedException();
		}
	}
}
