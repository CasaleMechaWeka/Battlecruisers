using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Targets.TargetFinders;
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

        private const float FRIEND_DETECTION_RADIUS_MULTIPLIER = 1.2f;

		public CircleTargetDetector enemyDetector, friendDetector;

        protected abstract float EnemyDetectionRangeInM { get; }
		public override TargetType TargetType { get { return TargetType.Ships; } }

		private float _damage;
		public sealed override float Damage { get { return _damage; } }

        private float FriendDetectionRangeInM
        {
            get
            {
                return FRIEND_DETECTION_RADIUS_MULTIPLIER * Size.x / 2;
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
			
            UpdateVelocity();
        }

        private void SetupBlockingUnitDetection()
        {
            // Detect blocking enemies
            enemyDetector.Initialise(EnemyDetectionRangeInM);
            _blockingEnemyProvider = _targetsFactory.CreateShipBlockingEnemyProvider(enemyDetector, this);
            _blockingEnemyProvider.TargetChanged += (sender, e) => UpdateVelocity();

            // Friend detection for stopping
            friendDetector.Initialise(FriendDetectionRangeInM);
            _blockingFriendlyProvider = _targetsFactory.CreateShipBlockingFriendlyProvider(friendDetector, this);
            _blockingFriendlyProvider.TargetChanged += (sender, e) => UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            Assert.IsTrue(BuildableState == BuildableState.Completed);

            if (IsStationary)
            {
                if (_blockingEnemyProvider.Target == null
                    && _blockingFriendlyProvider.Target == null)
                {
                    StartMoving();
                }
            }
            else if (_blockingEnemyProvider.Target != null
                || _blockingFriendlyProvider.Target != null)
            {
                StopMoving();
            }
        }

		private void StartMoving()
		{
			Logging.Log(Tags.ATTACK_BOAT, "StartMoving()");
			rigidBody.velocity = new Vector2(maxVelocityInMPerS * _directionMultiplier, 0);
		}

		private void StopMoving()
		{
			Logging.Log(Tags.ATTACK_BOAT, "StopMoving()");
			rigidBody.velocity = new Vector2(0, 0);
		}

		// Enemy detector is in ship center, but longest range barrel may be behind
		// ship center.  Want to only stop once barrel is in range, so make enemy 
        // detection range be less than the longest range barrel.
        protected float FindEnemyDetectionRange(IBarrelWrapper longestRangeBarrel)
        {
            return longestRangeBarrel.RangeInM - (Mathf.Abs(transform.position.x - longestRangeBarrel.Position.x));
        }
	}
}
