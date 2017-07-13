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

		// FELIX  Remove unused
		private float _patrollingSmoothTime;
		protected Vector2 _patrollingVelocity;
		protected bool _isPatrolling;
		private Vector2 _lastPatrolPoint;
		private Vector2 _targetPatrolPoint;

		private const float POSITION_EQUALITY_MARGIN = 0.1f;
		private const float SMOOTH_TIME_MULTIPLIER = 2;
		private const float DEFAULT_SMOOTH_TIME_IN_S = 1;

		#region Properties
		public override TargetType TargetType { get { return TargetType.Aircraft; } }

		public IList<Vector2> PatrolPoints { protected get; set; }

		public override Vector2 Velocity
		{
			get
			{
				Logging.Log(Tags.AIRCRAFT, string.Format("_isPatrolling: {0}  _patrollingVelocity: {1}  base.Velocity: {2}", _isPatrolling, _patrollingVelocity, base.Velocity));

				return _isPatrolling ? _patrollingVelocity : base.Velocity;
			}
		}

		protected virtual float PatrollingVelocity { get { return maxVelocityInMPerS; } }
		#endregion Properties

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
