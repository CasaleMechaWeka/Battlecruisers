using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class GunshipController : AircraftController, ITargetConsumer
	{
        private ITargetFinder _hoveringTargetFinder;
        private IMovementController _hoverMovementController;
        private FollowingXAxisMovementController _followingMovementController;
        private IBarrelWrapper _barrelWrapper;
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
                _followingMovementController.Target = _target;

                UpdateMovementController();
			}
		}

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(hoverRangeEnemyDetector);

            _attackCapabilities.Add(TargetType.Ships);

            _barrelWrapper = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_barrelWrapper);
			_barrelWrapper.StaticInitialise();

            _targetProcessorWrapper = transform.Find("TargetProcessor").gameObject.GetComponent<ProximityTargetProcessorWrapper>();
            Assert.IsNotNull(_targetProcessorWrapper);
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

            _hoverMovementController = _movementControllerFactory.CreateHoveringMovementController(rigidBody, maxVelocityInMPerS);
            _followingMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityInMPerS);

            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _barrelWrapper.Initialise(_factoryProvider, enemyFaction, AttackCapabilities);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

            // Create target processor => For following enemies
            ITargetConsumer targetConsumer = this;
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            _targetProcessorWrapper
                .StartProvidingTargets(
                    _factoryProvider.TargetsFactory,
                    targetConsumer,
                    enemyFaction,
                    enemyFollowRangeInM,
                    AttackCapabilities);

			// Create target tracker => For hovering over enemies
            hoverRangeEnemyDetector.Initialise(enemyHoverRangeInM);
            ITargetFilter enemyDetectionFilter = _factoryProvider.TargetsFactory.CreateTargetFilter(enemyFaction, AttackCapabilities);
            _hoveringTargetFinder = _factoryProvider.TargetsFactory.CreateRangedTargetFinder(hoverRangeEnemyDetector, enemyDetectionFilter);
            _hoverRangeTargetTracker = _factoryProvider.TargetsFactory.CreateTargetTracker(_hoveringTargetFinder);
            _hoverRangeTargetTracker.TargetsChanged += _hoverRangeTargetTracker_TargetsChanged;
            _hoveringTargetFinder.StartFindingTargets();

            _barrelWrapper.StartAttackingTargets();
		}

        private void _hoverRangeTargetTracker_TargetsChanged(object sender, EventArgs e)
        {
            Logging.Log(Tags.AIRCRAFT, "GunshipController._hoverRangeTargetTracker_TargetsChanged()");
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
