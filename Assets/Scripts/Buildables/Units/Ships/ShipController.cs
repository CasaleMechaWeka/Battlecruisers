using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    /// <summary>
    /// Assumptions:
    /// 1. Boats only move horizontally, and are all at the same height
    /// 2. All enemies will come towards the front of the boat, and all allies will come
    ///     towards the rear of the boat.
    /// 3. Boat will only stop to fight enemies (or to avoid bumping into friendlies).
    ///     Either this boat is destroyed, or the enemy, in which case this boat will continue moving.
    /// </summary>
    public abstract class ShipController : Unit
	{
		private int _directionMultiplier;
        private IList<IBarrelWrapper> _turrets;
        private IBroadCastingTargetProvider _blockingEnemyProvider, _blockingFriendlyProvider;
        private IHighestPriorityTargetProvider _highPriorityTarget;
        private ITargetProvider _highestPriorityTargetProvider;
        private TargetProcessorWrapper _targetProcessorWrapper;

        private const float FRIEND_DETECTION_RADIUS_MULTIPLIER = 1.2f;
        private const float ENEMY_DETECTION_RADIUS_MULTIPLIER = 2;

		public CircleTargetDetector enemyDetector, friendDetector;

        public override TargetType TargetType { get { return TargetType.Ships; } }

        /// <summary>
        /// Optimal range for ship to do the most damage, while staying out of
        /// range of defence buildings.
        /// 
        /// Usually this will simply be the range of the ship's longest ranged turret,
        /// but can be less if we want multiple of the ships turrets to be in range.
        /// </summary>
        protected abstract float OptimalArmamentRangeInM { get; }

        private float _damage;
        public sealed override float Damage { get { return _damage; } }

        private float FriendDetectionRangeInM
        {
            get
            {
                return FRIEND_DETECTION_RADIUS_MULTIPLIER * Size.x / 2;
            }
        }

		private float EnemyDetectionRangeInM
        {
            get
            {
                return ENEMY_DETECTION_RADIUS_MULTIPLIER * Size.x / 2;
            }
        }

        private bool IsStationary { get { return rigidBody.velocity.x == 0; } }

        public override void StaticInitialise()
		{
			base.StaticInitialise();

			_attackCapabilities.Add(TargetType.Ships);
			_attackCapabilities.Add(TargetType.Cruiser);
			_attackCapabilities.Add(TargetType.Buildings);

            _turrets = GetTurrets();

			foreach (IBarrelWrapper turret in _turrets)
            {
                turret.StaticInitialise();
            }

			_damage = _turrets.Sum(turret => turret.DamagePerS);

            _targetProcessorWrapper = transform.FindNamedComponent<TargetProcessorWrapper>("ShipTargetProcessorWrapper");
		}

        protected abstract IList<IBarrelWrapper> GetTurrets();

		protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            _directionMultiplier = FacingDirection == Direction.Right ? 1 : -1;

            // Initialise turrets
            foreach (IBarrelWrapper turret in _turrets)
            {
                turret.StartAttackingTargets();
            }

            SetupBlockingUnitDetection();
			SetupTargetProcessorWrapper();
			
            UpdateVelocity();
        }

        private void SetupTargetProcessorWrapper()
        {
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            ITargetProcessorArgs args 
                = new TargetProcessorArgs(
                    _factoryProvider.TargetsFactory,
                    _highPriorityTarget,
                    enemyFaction,
                    _attackCapabilities,
                    OptimalArmamentRangeInM);

			_targetProcessorWrapper.Initialise(args);
            _targetProcessorWrapper.StartProvidingTargets();
        }

        private void SetupBlockingUnitDetection()
        {
            // Detect blocking enemies
            enemyDetector.Initialise(EnemyDetectionRangeInM);
            _blockingEnemyProvider = _targetsFactory.CreateShipBlockingEnemyProvider(enemyDetector, this);
            _blockingEnemyProvider.TargetChanged += OnTargetChanged;

            // Friend detection for stopping
            friendDetector.Initialise(FriendDetectionRangeInM);
            _blockingFriendlyProvider = _targetsFactory.CreateShipBlockingFriendlyProvider(friendDetector, this);
            _blockingFriendlyProvider.TargetChanged += OnTargetChanged;

            // High priority target provider, for detecting targets that are attacking
            // us but are currently out of range.
            ITargetRanker shipTargetRanker = _targetsFactory.CreateShipTargetRanker();
            _highPriorityTarget = _targetsFactory.CreateHighestPriorityTargetProvider(shipTargetRanker, this);
            _highestPriorityTargetProvider = _highPriorityTarget;
            _highPriorityTarget.TargetChanged += OnTargetChanged;
            _highPriorityTarget.NewInRangeTarget += OnTargetChanged;
        }

        private void OnTargetChanged(object sender, EventArgs args)
        {
            UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            Assert.IsTrue(BuildableState == BuildableState.Completed);

            if (IsStationary)
            {
                if (_blockingEnemyProvider.Target == null
                    && _blockingFriendlyProvider.Target == null
                    && (_highestPriorityTargetProvider.Target == null
                        || !IsHighestPriorityTargetWithinRange()))
                {
                    StartMoving();
                }
            }
            else if (_blockingEnemyProvider.Target != null
                || _blockingFriendlyProvider.Target != null
                || (_highestPriorityTargetProvider.Target != null
                    && IsHighestPriorityTargetWithinRange()))
            {
                StopMoving();
            }
        }

        private bool IsHighestPriorityTargetWithinRange()
        {
            Assert.IsTrue(_highestPriorityTargetProvider.Target != null);
            float distanceCenterToCenter = Vector2.Distance(_highestPriorityTargetProvider.Target.Position, Position);
            float distanceCenterToEdge = distanceCenterToCenter - _highestPriorityTargetProvider.Target.Size.x / 2;

            Logging.Log(Tags.SHIPS, "Distance: " + distanceCenterToEdge + "  Range: " + OptimalArmamentRangeInM);

            return distanceCenterToEdge <= OptimalArmamentRangeInM;
        }

		private void StartMoving()
		{
			Logging.Log(Tags.SHIPS, "StartMoving()");
			rigidBody.velocity = new Vector2(maxVelocityInMPerS * _directionMultiplier, 0);
		}

		private void StopMoving()
		{
			Logging.Log(Tags.SHIPS, "StopMoving()");
			rigidBody.velocity = new Vector2(0, 0);
		}

        /// <summary>
		/// Enemy detector is in ship center, but longest range barrel may be behind
		/// ship center.  Want to only stop once barrel is in range, so make optimal 
		/// armament range be less than the longest range barrel.
        protected float FindOptimalArmamentRangeInM(IBarrelWrapper longestRangeBarrel)
        {
            return longestRangeBarrel.RangeInM - (Mathf.Abs(transform.position.x - longestRangeBarrel.Position.x));
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            if (BuildableState == BuildableState.Completed)
            {
                _blockingEnemyProvider.TargetChanged -= OnTargetChanged;
                _blockingFriendlyProvider.TargetChanged -= OnTargetChanged;
                _highPriorityTarget.TargetChanged -= OnTargetChanged;
                _highPriorityTarget.NewInRangeTarget -= OnTargetChanged;
            }
        }
	}
}
