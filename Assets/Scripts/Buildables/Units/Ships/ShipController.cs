using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ManualDetectorProvider _enemyDetectorProvider, _friendDetectorProvider;

        private const float FRIEND_DETECTION_RADIUS_MULTIPLIER = 1.2f;
        private const float ENEMY_DETECTION_RADIUS_MULTIPLIER = 2;

        public override TargetType TargetType => TargetType.Ships;
        protected override ISoundKey DeathSoundKey => SoundKeys.Deaths.Ship;
        protected override float OnDeathGravityScale => 0.2f;

        /// <summary>
        /// Optimal range for ship to do the most damage, while staying out of
        /// range of defence buildings.
        /// 
        /// Usually this will simply be the range of the ship's longest ranged turret,
        /// but can be less if we want multiple of the ships turrets to be in range.
        /// </summary>
        public abstract float OptimalArmamentRangeInM { get; }

        private float FriendDetectionRangeInM => FRIEND_DETECTION_RADIUS_MULTIPLIER * Size.x / 2;
		private float EnemyDetectionRangeInM => ENEMY_DETECTION_RADIUS_MULTIPLIER * Size.x / 2;
        public bool IsMoving => rigidBody.velocity.x != 0;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

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

        protected override void AddBuildRateBoostProviders(
            IGlobalBoostProviders globalBoostProviders, 
            IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(globalBoostProviders.UnitBuildRate.ShipProviders);
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
                    _cruiserSpecificFactories,
                    _factoryProvider.Targets,
                    enemyFaction,
                    targetProcessorTargetTypes,
                    OptimalArmamentRangeInM,
                    parentTarget: this);

			return _targetProcessorWrapper.CreateTargetProcessor(args);
        }

        private IMovementDecider SetupMovementDecider(ITargetFinder inRangeTargetFinder)
        {
            IRangeCalculator rangeCalculator = _factoryProvider.Targets.RangeCalculatorProvider.SizeInclusiveCalculator;
            _enemyDetectorProvider
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipTargetDetector(
                    Transform,
                    EnemyDetectionRangeInM,
                    rangeCalculator);
            _friendDetectorProvider
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateFriendlyShipTargetDetector(
                    Transform,
                    FriendDetectionRangeInM,
                    rangeCalculator);

            return
                _movementControllerFactory.CreateShipMovementDecider(
                    this,
                    _cruiserSpecificFactories.Targets.ProviderFactory.CreateShipBlockingEnemyProvider(_enemyDetectorProvider.TargetDetector, this),
                    _cruiserSpecificFactories.Targets.ProviderFactory.CreateShipBlockingFriendlyProvider(_friendDetectorProvider.TargetDetector, this),
                    _cruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(inRangeTargetFinder),
                    _enemyCruiser.BlockedShipsTracker,
                    _targetFactories.HelperFactory.CreateShipRangeHelper(this));
        }

		public void StartMoving()
		{
			Logging.LogMethod(Tags.SHIPS);
			rigidBody.velocity = new Vector2(maxVelocityInMPerS * _directionMultiplier, 0);
		}

		public void StopMoving()
		{
            Logging.LogMethod(Tags.SHIPS);
			rigidBody.velocity = new Vector2(0, 0);
		}

        protected override void OnDestroyed()
        {
            CleanUp();
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

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = base.GetInGameRenderers();

            foreach (IBarrelWrapper turret in _turrets)
            {
                renderers.AddRange(turret.Renderers);
            }

            return renderers;
        }

        public void DisableMovement()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            if (BuildableState == BuildableState.Completed)
            {
                _movementDecider.DisposeManagedState();
                _movementDecider = null;

                _movementTargetProcessor.DisposeManagedState();
                _movementTargetProcessor = null;

                _enemyDetectorProvider.DisposeManagedState();
                _enemyDetectorProvider = null;

                _friendDetectorProvider.DisposeManagedState();
                _friendDetectorProvider = null;
            }
        }
    }
}
