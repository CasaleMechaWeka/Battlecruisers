using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class GunShipController : AircraftController, ITargetConsumer
	{
        private FollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private IBarrelWrapper _barrelWrapper;
        private TargetProcessorWrapper _followingTargetProcessor;
		private ITargetFinder _inRangeTargetFinder;
        private ITargetTracker _inRangeTargetTracker;
		private bool _isAtCruisingHeight;

		private const float WITHTIN_RANGE_MULTIPLIER = 0.5f;

		public CircleTargetDetector hoverRangeEnemyDetector;
        public float enemyHoverRangeInM, enemyFollowRangeInM;

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

        protected override ISoundKey EngineSoundKey { get { return SoundKeys.Engines.Gunship; } }

		public override void StaticInitialise()
		{
			base.StaticInitialise();

			Assert.IsNotNull(hoverRangeEnemyDetector);

            _attackCapabilities.Add(TargetType.Ships);

            _barrelWrapper = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_barrelWrapper);
			_barrelWrapper.StaticInitialise();
            _damageStats.Add(_barrelWrapper.Damage);

            _followingTargetProcessor = transform.FindNamedComponent<ProximityTargetProcessorWrapper>("FollowingTargetProcessor");

            _isAtCruisingHeight = false;
		}

		protected override void OnInitialised()
		{
			base.OnInitialised();

            _outsideRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IVelocityProvider inRangeVelocityProvider
                = _movementControllerFactory.CreateMultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHTIN_RANGE_MULTIPLIER);
            _inRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);

            Faction enemyFaction = Helper.GetOppositeFaction(Faction);
            _barrelWrapper.Initialise(this, _factoryProvider, enemyFaction, AttackCapabilities, SoundKeys.Firing.BigCannon);
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

            // Create target processor => For following enemies
            ITargetConsumer targetConsumer = this;
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            ITargetProcessorArgs args
                = new TargetProcessorArgs(
                    _factoryProvider.TargetsFactory,
                    targetConsumer,
                    enemyFaction,
                    AttackCapabilities,
                    enemyFollowRangeInM);

            _followingTargetProcessor.Initialise(args);
            _followingTargetProcessor.StartProvidingTargets();

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
            IList<Vector2> patrolPositions = _aircraftProvider.FindGunshipPatrolPoints(cruisingAltitudeInM);
            return ProcessPatrolPoints(patrolPositions, OnFirstPatrolPointReached);
		}

		private void OnFirstPatrolPointReached()
		{
			_isAtCruisingHeight = true;
            UpdateMovementController();
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
            if (_isAtCruisingHeight && Target != null)
			{
                return _inRangeTargetTracker.ContainsTarget(Target) ? _inRangeMovementController : _outsideRangeMovementController;
			}
            return PatrollingMovementController;
        }

		private void CleanUp()
		{
            _followingTargetProcessor.Dispose();
            _followingTargetProcessor = null;

            _inRangeTargetTracker.TargetsChanged -= _hoverRangeTargetTracker_TargetsChanged;
            _inRangeTargetFinder.Dispose();
            _inRangeTargetFinder = null;

            _inRangeTargetTracker.Dispose();
            _inRangeTargetTracker = null;
		}
	}
}
