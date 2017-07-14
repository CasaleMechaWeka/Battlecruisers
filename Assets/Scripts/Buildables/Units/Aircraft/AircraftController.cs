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

		protected virtual float MaxPatrollingVelocity { get { return maxVelocityInMPerS; } }

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_patrollingMovementController = _movementControllerFactory.CreatePatrollingMovementController(rigidBody, MaxPatrollingVelocity, GetPatrolPoints());
			_patrollingMovementController.DirectionChanged += _movementController_DirectionChanged;
			_activeMovementController = _patrollingMovementController;
		}

		protected abstract IList<IPatrolPoint> GetPatrolPoints();

		private void _movementController_DirectionChanged(object sender, XDirectionChangeEventArgs e)
		{
			FacingDirection = e.NewDirection;
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			Assert.IsNotNull(_activeMovementController, "OnInitialised() should always be called before OnFixedUpdate()");
			_activeMovementController.AdjustVelocity();
		}

		protected void SwitchMovementControllers(IMovementController newMovementController)
		{
			newMovementController.Velocity = _activeMovementController.Velocity;
			_activeMovementController.Velocity = new Vector2(0, 0);
			_activeMovementController = newMovementController;
		}
	}
}
