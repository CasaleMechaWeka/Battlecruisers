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
        private FollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private IBarrelWrapper _barrelWrapper;
        private ITargetProcessorWrapper _targetProcessorWrapper;
		private ITargetFinder _inRangeTargetFinder;
        private ITargetTracker _inRangeTargetTracker;

        private const float WITHTIN_RANGE_MULTIPLIER = 0.5f;

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
                _outsideRangeMovementController.Target = _target;
                _inRangeMovementController.Target = _target;

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

            _outsideRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityInMPerS);
			_inRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityInMPerS * WITHTIN_RANGE_MULTIPLIER);

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

			// Create target tracker => For keeping track of in range targets
            hoverRangeEnemyDetector.Initialise(enemyHoverRangeInM);
            ITargetFilter enemyDetectionFilter = _factoryProvider.TargetsFactory.CreateTargetFilter(enemyFaction, AttackCapabilities);
            _inRangeTargetFinder = _factoryProvider.TargetsFactory.CreateRangedTargetFinder(hoverRangeEnemyDetector, enemyDetectionFilter);
            _inRangeTargetTracker = _factoryProvider.TargetsFactory.CreateTargetTracker(_inRangeTargetFinder);
            _inRangeTargetTracker.TargetsChanged += _hoverRangeTargetTracker_TargetsChanged;
            _inRangeTargetFinder.StartFindingTargets();

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

            if (!ReferenceEquals(ActiveMovementController, desiredMovementController))
            {
                SwitchMovementControllers(desiredMovementController);
            }
        }

        private IMovementController ChooseMovementController()
        {
			if (Target != null)
			{
                return _inRangeTargetTracker.ContainsTarget(Target) ? _inRangeMovementController : _outsideRangeMovementController;
			}
            return PatrollingMovementController;
        }

		private void CleanUp()
		{
            _targetProcessorWrapper.Dispose();
            _targetProcessorWrapper = null;

            _inRangeTargetTracker.TargetsChanged -= _hoverRangeTargetTracker_TargetsChanged;
            _inRangeTargetFinder.Dispose();
            _inRangeTargetFinder = null;

            _inRangeTargetTracker.Dispose();
            _inRangeTargetTracker = null;
		}
	}
}
