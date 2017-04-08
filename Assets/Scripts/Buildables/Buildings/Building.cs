using BattleCruisers.Cruisers;
using BattleCruisers.Buildings.Turrets;
using BattleCruisers.UI;
using BattleCruisers.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.Buildings
{
	public enum BuildingCategory
	{
		Factory, Defence, Offence, Tactical, Support, Ultras
	}

	public class Building : Buildable, IPointerClickHandler
	{
		public BuildingCategory category;
		// Proportional to building size
		public float customOffsetProportion;

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.SelectBuilding(this, _parentCruiser);
			OnClicked();
		}

		protected virtual void OnClicked() { }

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
