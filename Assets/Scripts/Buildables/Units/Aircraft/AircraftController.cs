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

		public override Vector2 Velocity { get { return _movementController.Velocity; } }

		protected virtual float PatrollingVelocity { get { return maxVelocityInMPerS; } }

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_movementController = _movementControllerFactory.CreatePatrollingMovementController(rigidBody, maxVelocityInMPerS, PatrolPoints);
			_movementController.DirectionChanged += _movementController_DirectionChanged;
		}

		protected virtual void _movementController_DirectionChanged(object sender, XDirectionChangeEventArgs e)
		{
			FacingDirection = e.NewDirection;
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			if (_movementController != null)
			{
				_movementController.AdjustVelocity();
			}
		}
	}
}
