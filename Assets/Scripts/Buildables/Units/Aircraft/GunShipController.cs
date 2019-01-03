using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class GunShipController : AircraftController, ITargetConsumer
	{
        private FollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private IBarrelWrapper _barrelWrapper;
        private ITargetProcessor _followingTargetProcessor;
        private ITargetFinder _inRangeTargetFinder;
        private ITargetTracker _inRangeTargetTracker;
		private bool _isAtCruisingHeight;

		private const float WITHTIN_RANGE_MULTIPLIER = 0.5f;

		public CircleTargetDetectorController hoverRangeEnemyDetector;
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
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Units.Gunship; } }

        protected override void OnStaticInitialised()
		{
            base.OnStaticInitialised();

            Assert.IsNotNull(hoverRangeEnemyDetector);

            _barrelWrapper = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_barrelWrapper);
			_barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);

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
		}

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

            // Create target processor => For following enemies
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            ITargetProcessorArgs args
                = new TargetProcessorArgs(
                    _factoryProvider.TargetFactories,
                    enemyFaction,
                    AttackCapabilities,
                    enemyFollowRangeInM);

            TargetProcessorWrapper followingTargetProcessorWrapper = transform.FindNamedComponent<ProximityTargetProcessorWrapper>("FollowingTargetProcessor");
            _followingTargetProcessor = followingTargetProcessorWrapper.CreateTargetProcessor(args);
            _followingTargetProcessor.AddTargetConsumer(this);

            // Create target tracker => For keeping track of in range targets
            hoverRangeEnemyDetector.Initialise(enemyHoverRangeInM);
            ITargetFilter enemyDetectionFilter = _factoryProvider.TargetsFactory.CreateTargetFilter(enemyFaction, AttackCapabilities);
            _inRangeTargetFinder = _factoryProvider.TargetsFactory.CreateRangedTargetFinder(hoverRangeEnemyDetector, enemyDetectionFilter);
            _inRangeTargetTracker = _factoryProvider.TargetsFactory.CreateTargetTracker(_inRangeTargetFinder);
            _inRangeTargetTracker.TargetsChanged += _hoverRangeTargetTracker_TargetsChanged;

            _barrelWrapper.Initialise(this, _factoryProvider, enemyFaction, SoundKeys.Firing.BigCannon);
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
            ActiveMovementController = ChooseMovementController();
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
            _followingTargetProcessor.DisposeManagedState();

            _inRangeTargetTracker.TargetsChanged -= _hoverRangeTargetTracker_TargetsChanged;
            _inRangeTargetFinder.DisposeManagedState();

            _inRangeTargetTracker.DisposeManagedState();
		}
	}
}
