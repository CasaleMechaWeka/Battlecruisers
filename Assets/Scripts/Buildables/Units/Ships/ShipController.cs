using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;

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
    public abstract class ShipController : Unit, IShip
	{
		private int _directionMultiplier;
        private IList<IBarrelWrapper> _turrets;
        private TargetProcessorWrapper _targetProcessorWrapper;
        private IMovementDecider _movementDecider;

        private const float FRIEND_DETECTION_RADIUS_MULTIPLIER = 1.2f;
        private const float ENEMY_DETECTION_RADIUS_MULTIPLIER = 2;

		public CircleTargetDetector enemyDetector, friendDetector;

        public override TargetType TargetType { get { return TargetType.Ships; } }
        protected override ISoundKey DeathSoundKey { get { return SoundKeys.Deaths.Ship; } }

        /// <summary>
        /// Optimal range for ship to do the most damage, while staying out of
        /// range of defence buildings.
        /// 
        /// Usually this will simply be the range of the ship's longest ranged turret,
        /// but can be less if we want multiple of the ships turrets to be in range.
        /// </summary>
        public abstract float OptimalArmamentRangeInM { get; }

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

        public bool IsMoving { get { return rigidBody.velocity.x != 0; } }

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

            FindDamageStats();

            _targetProcessorWrapper = transform.FindNamedComponent<TargetProcessorWrapper>("ShipTargetProcessorWrapper");
        }

        private void FindDamageStats()
        {
            IList<IDamage> antiAirDamageStats = GetDamageStats(TargetType.Aircraft);
            if (antiAirDamageStats.Count != 0)
            {
                _damageStats.Add(new Damage(antiAirDamageStats));
            }

            IList<IDamage> antiSeaDamageStats = GetDamageStats(TargetType.Ships);
            if (antiSeaDamageStats.Count != 0)
            {
                _damageStats.Add(new Damage(antiSeaDamageStats));
            }
        }

        private IList<IDamage> GetDamageStats(TargetType attackCapability)
        {
            return
                _turrets
                    .Where(turret => turret.Damage.AttackCapabilities.Contains(attackCapability))
                    .Select(turret => turret.Damage)
                    .ToList();
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

            _movementDecider = SetupMovementDecider();
			SetupTargetProcessorWrapper();
        }

        private void SetupTargetProcessorWrapper()
        {
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            // Do not want to stop ship from moving if it encounters aircraft
            IList<TargetType> targetProcessorTargetTypes = _attackCapabilities.ToList();
            targetProcessorTargetTypes.Remove(TargetType.Aircraft);

            ITargetProcessorArgs args 
                = new TargetProcessorArgs(
                    _factoryProvider.TargetsFactory,
                    _movementDecider,
                    enemyFaction,
                    targetProcessorTargetTypes,
                    OptimalArmamentRangeInM);

			_targetProcessorWrapper.Initialise(args);
            _targetProcessorWrapper.StartProvidingTargets();
        }

        private IMovementDecider SetupMovementDecider()
        {
            enemyDetector.Initialise(EnemyDetectionRangeInM);
            friendDetector.Initialise(FriendDetectionRangeInM);

            return
                _movementControllerFactory.CreateShipMovementDecider(
                    this,
                    _targetsFactory,
                    enemyDetector,
                    friendDetector);
        }

		public void StartMoving()
		{
			Logging.Log(Tags.SHIPS, "StartMoving()");
			rigidBody.velocity = new Vector2(maxVelocityInMPerS * _directionMultiplier, 0);
		}

		public void StopMoving()
		{
			Logging.Log(Tags.SHIPS, "StopMoving()");
			rigidBody.velocity = new Vector2(0, 0);
		}

        protected override void OnDestroyed()
        {
			if (_movementDecider != null)
			{
				_movementDecider.DisposeManagedState();
			}
            base.OnDestroyed();
		}
	}
}
