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
	public abstract class AircraftController : Unit
	{
		protected IMovementController _activeMovementController;
		protected IMovementController _patrollingMovementController;

		public override TargetType TargetType { get { return TargetType.Aircraft; } }

		public override Vector2 Velocity { get { return _activeMovementController.Velocity; } }

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_patrollingMovementController = _movementControllerFactory.CreatePatrollingMovementController(rigidBody, maxVelocityInMPerS, GetPatrolPoints());
			_patrollingMovementController.DirectionChanged += _movementController_DirectionChanged;
			_activeMovementController = _patrollingMovementController;
		}

		protected abstract IList<Vector2> GetPatrolPoints();

		protected virtual void _movementController_DirectionChanged(object sender, XDirectionChangeEventArgs e)
		{
			FacingDirection = e.NewDirection;
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			if (_activeMovementController != null)
			{
				_activeMovementController.AdjustVelocity();
			}
		}
	}
}
