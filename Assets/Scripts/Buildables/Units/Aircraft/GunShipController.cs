using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class GunShipController : AircraftController, ITargetConsumer
	{
        private ITargetFinder _hoveringTargetFinder;
        private IMovementController _hoverMovementController, _followingMovementController;
		private BarrelController _barrelController;
		private ITargetProcessorWrapper _targetProcessorWrapper;
        private ITargetTracker _hoverRangeTargetTracker;

		public CircleTargetDetector hoverRangeEnemyDetector;

        public float enemyHoverRangeInM, enemyFollowRangeInM;
		public float cruisingAltitudeInM;

		private ITarget _target;
		public ITarget Target
		{
			get { return _target; }
			set
			{
				_target = value;
                UpdateMovementController();
			}
		}

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(hoverRangeEnemyDetector);

            _attackCapabilities.Add(TargetType.Ships);

			_barrelController = gameObject.GetComponentInChildren<BarrelController>();
			Assert.IsNotNull(_barrelController);
			_barrelController.StaticInitialise();

            _targetProcessorWrapper = transform.Find("TargetProcessor").gameObject.GetComponent<ProximityTargetProcessorWrapper>();
            Assert.IsNotNull(_targetProcessorWrapper);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

            _hoverMovementController = _movementControllerFactory.CreateHoveringMovementController(rigidBody, maxVelocityInMPerS);
            _followingMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityInMPerS);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

            ITargetConsumer targetConsumer = this;
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            _targetProcessorWrapper
                .StartProvidingTargets(
                    _factoryProvider.TargetsFactory,
                    targetConsumer,
                    enemyFaction,
                    enemyFollowRangeInM,
                    AttackCapabilities);

			// Create target tracker
            hoverRangeEnemyDetector.Initialise(enemyHoverRangeInM);
            ITargetFilter enemyDetectionFilter = _factoryProvider.TargetsFactory.CreateTargetFilter(enemyFaction, AttackCapabilities);
            _hoveringTargetFinder = _factoryProvider.TargetsFactory.CreateRangedTargetFinder(hoverRangeEnemyDetector, enemyDetectionFilter);
            _hoverRangeTargetTracker = _factoryProvider.TargetsFactory.CreateTargetTracker(_hoveringTargetFinder);

            _hoverRangeTargetTracker.TargetsChanged += _hoverRangeTargetTracker_TargetsChanged;
		}

        private void _hoverRangeTargetTracker_TargetsChanged(object sender, EventArgs e)
        {
            UpdateMovementController();
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
		{
            return Helper.ConvertVectorsToPatrolPoints(_aircraftProvider.FindGunshipPatrolPoints(cruisingAltitudeInM));
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			if (BuildableState == BuildableState.Completed
				&& !IsInKamikazeMode)
			{
				CleanUp();
			}
		}

		protected override void OnKamikaze()
		{
			CleanUp();
		}

        private void UpdateMovementController()
        {
            IMovementController desiredMovementController = ChooseMovementController();

            if (!ReferenceEquals(_activeMovementController, desiredMovementController))
            {
                SwitchMovementControllers(desiredMovementController);
            }
        }

        private IMovementController ChooseMovementController()
        {
			if (Target != null)
			{
                return _hoverRangeTargetTracker.ContainsTarget(Target) ? _hoverMovementController : _followingMovementController;
			}
            return _patrollingMovementController;
        }

		private void CleanUp()
		{
            _targetProcessorWrapper.Dispose();
            _targetProcessorWrapper = null;

            _hoverRangeTargetTracker.TargetsChanged -= _hoverRangeTargetTracker_TargetsChanged;
            _hoveringTargetFinder.Dispose();
            _hoveringTargetFinder = null;

            _hoverRangeTargetTracker.Dispose();
            _hoverRangeTargetTracker = null;
		}
	}
}
