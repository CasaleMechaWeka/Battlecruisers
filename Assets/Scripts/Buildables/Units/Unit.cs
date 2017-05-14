using BattleCruisers.Drones;
using System;
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

		// FELIX  Create UnitStats class?
		public float maxVelocityInMPerS;

		public Rigidbody2D rigidBody;

		#region Properties
		public IDroneConsumerProvider DroneConsumerProvider	{ set { _droneConsumerProvider = value;	} }
		public UnitCategory Category { get { return category; } }
		public override Vector2 Velocity { get { return rigidBody.velocity; } }

		private Direction _facingDirection;
		protected Direction FacingDirection
		{
			get { return _facingDirection; }
			set
			{
				_facingDirection = value;
				OnDirectionChange();
			}
		}
		#endregion Properties

		protected override void OnAwake()
		{
			base.OnAwake();

			Assert.IsTrue(maxVelocityInMPerS > 0);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

			FacingDirection = _parentCruiser.Direction;
		}

		void FixedUpdate()
		{
			OnFixedUpdate();
		}

		protected virtual void OnFixedUpdate() { }

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.ShowUnitDetails(this);
		}

		protected virtual void OnDirectionChange()
		{
			int yRotation = FindYRotation(FacingDirection);
			Quaternion rotation = gameObject.transform.rotation;
			rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, yRotation, rotation.eulerAngles.z);
			gameObject.transform.rotation = rotation;
		}

		private int FindYRotation(Direction facingDirection)
		{
			switch (facingDirection)
			{
				case Direction.Right:
					// Sprites by default are facing right, so DO NOT mirror
					return 0;
				case Direction.Left:
					// Sprites by default are facing right, so DO mirror
					return 180;
				default:
					throw new ArgumentException();
			}
		}
	}
}
