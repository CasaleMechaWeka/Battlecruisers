using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public class Unit : BuildableObject
	{
		public UnitCategory category;
		// FELIX  Remove?
		public Direction facingDirection;

		// FELIX  Only for ships!
		public float velocityInMPerS;

		void OnMouseDown()
		{
			_uiManager.ShowUnitDetails(this);
		}
	}
}
