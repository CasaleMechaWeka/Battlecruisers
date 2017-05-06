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

	public interface IUnit : ITarget
	{
		UnitCategory Category { get; }
	}

	public abstract class Unit : Buildable, IPointerClickHandler
	{
		public UnitCategory category;

		// FELIX  Remove?  Just figure out whether are mirrored?  (See turrets I think)
		// FELIX  Nah, deprecating mirroring the entir gameObject, only mirror sprites
//		public Direction facingDirection;

		// FELIX  Create UnitStats class?
		public float maxVelocityInMPerS;

		public Rigidbody2D rigidBody;

		public IDroneConsumerProvider DroneConsumerProvider	{ set { _droneConsumerProvider = value;	} }
		public UnitCategory Category { get { return category; } }
		public override Vector2 Velocity { get { return rigidBody.velocity; } }

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
