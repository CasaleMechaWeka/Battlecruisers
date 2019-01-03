using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
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
        private ShipTargetProcessorWrapper _targetProcessorWrapper;
        private ITargetProcessor _movementTargetProcessor;
        private IMovementDecider _movementDecider;

        private const float FRIEND_DETECTION_RADIUS_MULTIPLIER = 1.2f;
        private const float ENEMY_DETECTION_RADIUS_MULTIPLIER = 2;

		public CircleTargetDetectorController enemyDetector, friendDetector;

        public override TargetType TargetType { get { return TargetType.Ships; } }
        protected override ISoundKey DeathSoundKey { get { return SoundKeys.Deaths.Ship; } }
        protected override float OnDeathGravityScale { get { return 0.2f; } }

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

        protected override void OnStaticInitialised()
        {
            base.OnStaticInitialised();

            _turrets = GetTurrets();

            foreach (IBarrelWrapper turret in _turrets)
            {
                turret.StaticInitialise();
            }

            FindDamageStats();

            _targetProcessorWrapper = transform.FindNamedComponent<ShipTargetProcessorWrapper>("ShipTargetProcessorWrapper");
        }

        private void FindDamageStats()
        {
            IList<IDamageCapability> antiAirDamageCapabilities = GetDamageCapabilities(TargetType.Aircraft);
            if (antiAirDamageCapabilities.Count != 0)
            {
                AddDamageStats(new DamageCapability(antiAirDamageCapabilities));
            }

            IList<IDamageCapability> antiSeaDamageCapabilities = GetDamageCapabilities(TargetType.Ships);
            if (antiSeaDamageCapabilities.Count != 0)
            {
                AddDamageStats(new DamageCapability(antiSeaDamageCapabilities));
            }
        }

        private IList<IDamageCapability> GetDamageCapabilities(TargetType attackCapability)
        {
            return
                _turrets
                    .Where(turret => turret.DamageCapability.AttackCapabilities.Contains(attackCapability))
                    .Select(turret => turret.DamageCapability)
                    .ToList();
        }

        protected abstract IList<IBarrelWrapper> GetTurrets();

		protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            _directionMultiplier = FacingDirection == Direction.Right ? 1 : -1;

            InitialiseTurrets();

			_movementTargetProcessor = SetupTargetProcessorWrapper();
            _movementDecider = SetupMovementDecider(_targetProcessorWrapper.InRangeTargetFinder);
            _movementTargetProcessor.AddTargetConsumer(_movementDecider);
        }

        // Turrets start attacking targets as soon as they are initialised, so
        // only initialise them once the ship has been completed.
        protected abstract void InitialiseTurrets();

        private ITargetProcessor SetupTargetProcessorWrapper()
        {
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            // Do not want to stop ship from moving if it encounters aircraft
            IList<TargetType> targetProcessorTargetTypes = AttackCapabilities.ToList();
            targetProcessorTargetTypes.Remove(TargetType.Aircraft);

            ITargetProcessorArgs args 
                = new TargetProcessorArgs(
                    _factoryProvider.TargetFactories,
                    _factoryProvider.TargetsFactory,
                    enemyFaction,
                    targetProcessorTargetTypes,
                    OptimalArmamentRangeInM,
                    parentTarget: this);

			return _targetProcessorWrapper.CreateTargetProcessor(args);
        }

        private IMovementDecider SetupMovementDecider(ITargetFinder inRangeTargetFinder)
        {
            enemyDetector.Initialise(EnemyDetectionRangeInM);
            friendDetector.Initialise(FriendDetectionRangeInM);

            return
                _movementControllerFactory.CreateShipMovementDecider(
                    this,
                    _targetFactories.ProviderFactory.CreateShipBlockingEnemyProvider(enemyDetector, this),
                    _targetFactories.ProviderFactory.CreateShipBlockingFriendlyProvider(friendDetector, this),
                    _targetFactories.TrackerFactory.CreateTargetTracker(inRangeTargetFinder),
                    _targetFactories.HelperFactory.CreateShipRangeHelper(this));
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

            if (_movementTargetProcessor != null)
            {
                _movementTargetProcessor.DisposeManagedState();
            }

            base.OnDestroyed();
		}

        protected override void OnDeathWhileCompleted()
        {
            base.OnDeathWhileCompleted();

            StopMoving();

            // Disable turrets
            foreach (IBarrelWrapper turret in _turrets)
            {
                turret.DisposeManagedState();
            }

            // Make ship rear sink first
            rigidBody.AddTorque(0.75f, ForceMode2D.Impulse);
        }
    }
}
