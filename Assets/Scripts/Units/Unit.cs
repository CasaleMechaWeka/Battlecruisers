using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Units
{
	public enum UnitCategory
	{
		Naval, Aircraft, Ultra
	}

	public enum Direction
	{
		Left, Right, Up, Down
	}

	public class Unit : BuildableObject, IPointerClickHandler
	{
		public UnitCategory category;
		// FELIX  Remove?
		public Direction facingDirection;

		// FELIX  Only for ships!
		public float velocityInMPerS;

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.ShowUnitDetails(this);
		}
	}
}
