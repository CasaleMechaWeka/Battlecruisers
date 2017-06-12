using BattleCruisers.Cruisers;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Buildables.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.Buildables.Buildings
{
	public enum BuildingCategory
	{
		Factory, Defence, Offence, Tactical
	}

	public class Building : Buildable, IPointerClickHandler
	{
		public BuildingCategory category;
		// Proportional to building size
		public float customOffsetProportion;

		public override TargetType TargetType { get { return TargetType.Buildings; } }

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.SelectBuilding(this, _parentCruiser);
			OnClicked();
		}

		protected virtual void OnClicked() { }

		public override void InitiateDelete()
		{
			Destroy();
		}

		public void CancelDelete()
		{
			throw new NotImplementedException();
		}
	}
}
