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

		// FELIX  Remove?  Just figure out whether are mirrored?  (See turrets I think)
		// FELIX  Nah, deprecating mirroring the entir gameObject, only mirror sprites
//		public Direction facingDirection;

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
				Debug.Log("FacingDirection: " + value);
				_facingDirection = value;
				OnDirectionChange();
			}
		}

		private IList<SpriteRenderer> _spriteRenderers;
		private IList<SpriteRenderer> SpriteRenderers
		{
			get
			{
				if (_spriteRenderers == null)
				{
					_spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
				}
				return _spriteRenderers;
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

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.ShowUnitDetails(this);
		}

		protected virtual void OnDirectionChange()
		{
			// Make sprites face the right direction
			foreach (SpriteRenderer spriteRenderer in SpriteRenderers)
			{
				spriteRenderer.flipX = ShouldFlipXAxis(FacingDirection);
			}
		}

		private bool ShouldFlipXAxis(Direction facingDirection)
		{
			switch (facingDirection)
			{
				case Direction.Right:
					// Sprites by default are facing right, no flipping needed
					return false;
				case Direction.Left:
					// Need to flip x axis
					return true;
				default:
					throw new ArgumentException();
			}
		}
	}
}
