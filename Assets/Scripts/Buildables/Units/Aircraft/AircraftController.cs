using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Projectiles;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
	public class AircraftController : Unit
	{
		private IMovementController _movementController;

		public override TargetType TargetType { get { return TargetType.Aircraft; } }

		public IList<Vector2> PatrolPoints { protected get; set; }

		public Vector2 Velocity { get { return _movementController.Velocity; } }

		protected virtual float PatrollingVelocity { get { return maxVelocityInMPerS; } }

		protected override void OnInitialised()
		{
			base.OnInitialised();
			_movementController = _movementControllerFactory.CreatePatrollingMovementController(rigidBody, maxVelocityInMPerS, PatrolPoints);

			// FELIX  Facing direction!
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			if (_movementController != null)
			{
				_movementController.AdjustVelocity();
			}
		}

		// FELIX call :P
		protected void UpdateFacingDirection(Vector2 oldVelocity, Vector2 currentVelocity)
		{
			if (oldVelocity.x > 0 && currentVelocity.x < 0)
			{
				FacingDirection = Direction.Left;
			}
			else if (oldVelocity.x < 0 && currentVelocity.x > 0)
			{
				FacingDirection = Direction.Right;
			}
		}
	}
}
