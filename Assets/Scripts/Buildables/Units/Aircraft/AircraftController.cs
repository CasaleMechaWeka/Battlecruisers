using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public abstract class AircraftController : Unit
	{
		protected IMovementController _activeMovementController;
		protected IMovementController _dummyMovementController;
		protected IMovementController _patrollingMovementController;

		public override TargetType TargetType { get { return TargetType.Aircraft; } }

		public override Vector2 Velocity { get { return _activeMovementController.Velocity; } }

		protected virtual float MaxPatrollingVelocity { get { return maxVelocityInMPerS; } }

		protected override void OnInitialised()
		{
			base.OnInitialised();

			_dummyMovementController = _movementControllerFactory.CreateDummyMovementController();
			_activeMovementController = _dummyMovementController;

			_patrollingMovementController = _movementControllerFactory.CreatePatrollingMovementController(rigidBody, MaxPatrollingVelocity, GetPatrolPoints());
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

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
			_activeMovementController.DirectionChanged -= _movementController_DirectionChanged;

			_activeMovementController = newMovementController;
			_activeMovementController.DirectionChanged += _movementController_DirectionChanged;
		}

        public void Kamikaze(ICruiser target)
        {
            ITargetProvider cruiserTarget = _targetsFactory.CreateStaticTargetProvider(target);
            SwitchMovementControllers(_movementControllerFactory.CreateHomingMovementController(rigidBody, maxVelocityInMPerS, cruiserTarget));
        }
	}
}
