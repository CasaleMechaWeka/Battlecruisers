using BattleCruisers.Drones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Buildables.Units
{
	public enum UnitCategory
	{
		Naval, Aircraft
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

		// FELIX  Create UnitStats class?
		public float maxVelocityInMPerS;

		// FELIX  Only for ships!
		public Rigidbody2D rigidBody;

		public IDroneConsumerProvider DroneConsumerProvider
		{
			set
			{
				_droneConsumerProvider = value;
			}
		}

		protected override void OnAwake()
		{
			base.OnAwake();

			Assert.IsTrue(maxVelocityInMPerS > 0);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.ShowUnitDetails(this);
		}
	}
}
