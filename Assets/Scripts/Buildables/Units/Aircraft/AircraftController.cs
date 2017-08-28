using System.Collections.Generic;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public abstract class AircraftController : Unit
	{
        private KamikazeController _kamikazeController;

        protected IMovementController _activeMovementController;
		protected IMovementController _dummyMovementController;
		protected IMovementController _patrollingMovementController;

        protected bool IsInKamikazeMode { get { return _kamikazeController.isActiveAndEnabled; } }
		
        public override TargetType TargetType { get { return TargetType.Aircraft; } }

		public override Vector2 Velocity { get { return _activeMovementController.Velocity; } }

		protected virtual float MaxPatrollingVelocity { get { return maxVelocityInMPerS; } }

		public override void StaticInitialise()
        {
            base.StaticInitialise();

            _kamikazeController = GetComponentInChildren<KamikazeController>(includeInactive: true);
            Assert.IsNotNull(_kamikazeController);
            Assert.IsFalse(IsInKamikazeMode);
        }

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

        public void Kamikaze(ITarget target)
        {
			Assert.AreEqual(UnitCategory.Aircraft, Category, "Only aircraft should kamikaze");
            Assert.AreEqual(BuildableState.Completed, BuildableState, "Only completed aircraft should kamikaze.");

            ITargetProvider cruiserTarget = _targetsFactory.CreateStaticTargetProvider(target);
            SwitchMovementControllers(_movementControllerFactory.CreateHomingMovementController(rigidBody, maxVelocityInMPerS, cruiserTarget));

            Faction = Helper.GetOppositeFaction(target.Faction);

            _kamikazeController.gameObject.SetActive(true);

            OnKamikaze();
        }

        protected virtual void OnKamikaze() { }
	}
}
