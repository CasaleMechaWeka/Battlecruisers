using BattleCruisers.Drones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Buildables.Units
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

		public float velocityInMPerS;

		// FELIX  Only for ships!
		public Rigidbody2D rigidBody;

		public IDroneConsumerProvider DroneConsumerProvider
		{
			set
			{
				_droneConsumerProvider = value;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.ShowUnitDetails(this);
		}
	}
}
