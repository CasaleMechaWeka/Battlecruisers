using BattleCruisers.Drones;
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

	public class Unit : Buildable, IPointerClickHandler
	{
		public UnitCategory category;
		// FELIX  Remove?
		public Direction facingDirection;

		// FELIX  Only for ships!
		public float velocityInMPerS;

		public void SpecificInitialisation(IDroneConsumerProvider droneConsumerProvider)
		{
			_droneConsumerProvider = droneConsumerProvider;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.ShowUnitDetails(this);
		}
	}
}
