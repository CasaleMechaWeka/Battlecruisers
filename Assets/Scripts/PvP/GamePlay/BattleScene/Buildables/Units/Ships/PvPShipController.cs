using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Deciders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships
{
    /// <summary>
    /// Assumptions:
    /// 1. Boats only move horizontally, and are all at the same height
    /// 2. All enemies will come towards the front of the boat, and all allies will come
    ///     towards the rear of the boat.
    /// 3. Boat will only stop to fight enemies (or to avoid bumping into friendlies).
    ///     Either this boat is destroyed, or the enemy, in which case this boat will continue moving.
    /// </summary>
    public abstract class PvPShipController : PvPUnit, IPvPShip
    {
        private int _directionMultiplier;
        private IList<IPvPBarrelWrapper> _turrets;
        private PvPShipTargetProcessorWrapper _targetProcessorWrapper;
        private IPvPTargetProcessor _movementTargetProcessor;
        private IPvPMovementDecider _movementDecider;
        private PvPManualDetectorProvider _enemyDetectorProvider, _friendDetectorProvider;
        private IPvPPool<IPvPShipDeath, Vector3> _deathPool;

        private float FRIEND_DETECTION_RADIUS_MULTIPLIER = 1.2f;
        private const float ENEMY_DETECTION_RADIUS_MULTIPLIER = 2;

        public override PvPTargetType TargetType => PvPTargetType.Ships;

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

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _turrets = GetTurrets();

            foreach (IPvPBarrelWrapper turret in _turrets)
            {
                turret.StaticInitialise();
            }

            FindDamageStats();

            _targetProcessorWrapper = transform.FindNamedComponent<PvPShipTargetProcessorWrapper>("ShipTargetProcessorWrapper");
        }

        protected virtual IList<IPvPBarrelWrapper> GetTurrets()
        {
            return new List<IPvPBarrelWrapper>();
        }

        private void FindDamageStats()
        {
            IList<IPvPDamageCapability> antiAirDamageCapabilities = GetDamageCapabilities(PvPTargetType.Aircraft);
            if (antiAirDamageCapabilities.Count != 0)
            {
                AddDamageStats(new PvPDamageCapability(antiAirDamageCapabilities));
            }

            IList<IPvPDamageCapability> antiSeaDamageCapabilities = GetDamageCapabilities(PvPTargetType.Ships);
            if (antiSeaDamageCapabilities.Count != 0)
            {
                AddDamageStats(new PvPDamageCapability(antiSeaDamageCapabilities));
            }
        }

        private IList<IPvPDamageCapability> GetDamageCapabilities(PvPTargetType attackCapability)
        {
            return
                _turrets
                    .Where(turret => turret.DamageCapability.AttackCapabilities.Contains(attackCapability))
                    .Select(turret => turret.DamageCapability)
                    .ToList();
        }

        public override void Initialise(/* IPvPUIManager uiManager, */ IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(/* uiManager ,*/ factoryProvider);

            IPvPShipDeathPoolChooser shipDeathPoolChooser = GetComponent<IPvPShipDeathPoolChooser>();
            Assert.IsNotNull(shipDeathPoolChooser);
            _deathPool = shipDeathPoolChooser.ChoosePool(factoryProvider.PoolProviders.ShipDeathPoolProvider);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            OnShipCompleted();
        }

        protected override void OnBuildableCompleted_PvPClient()
        {
            base.OnBuildableCompleted_PvPClient();
            OnShipCompleted();
        }
        protected virtual void OnShipCompleted()
        {
            InitialiseTurrets();
            SetupMovement();
        }

        // Turrets start attacking targets as soon as they are initialised, so
        // only initialise them once the ship has been completed.
        protected virtual void InitialiseTurrets() { }

        protected void SetupMovement()
        {
                _directionMultiplier = FacingDirection == PvPDirection.Right ? 1 : -1;
                _movementTargetProcessor = SetupTargetProcessorWrapper();
                _movementDecider = SetupMovementDecider(_targetProcessorWrapper.InRangeTargetFinder);
                _movementTargetProcessor.AddTargetConsumer(_movementDecider);
        }

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(globalBoostProviders.UnitBuildRate.ShipProviders);
        }

        private IPvPTargetProcessor SetupTargetProcessorWrapper()
        {
            PvPFaction enemyFaction = PvPHelper.GetOppositeFaction(Faction);

            // Do not want to stop ship from moving if it encounters aircraft
            IList<PvPTargetType> targetProcessorTargetTypes = AttackCapabilities.ToList();
            targetProcessorTargetTypes.Remove(PvPTargetType.Aircraft);

            IPvPTargetProcessorArgs args
                = new PvPTargetProcessorArgs(
                    _cruiserSpecificFactories,
                    _factoryProvider.Targets,
                    enemyFaction,
                    targetProcessorTargetTypes,
                    OptimalArmamentRangeInM,
                    parentTarget: this);

            return _targetProcessorWrapper.CreateTargetProcessor(args);
        }

        private IPvPMovementDecider SetupMovementDecider(IPvPTargetFinder inRangeTargetFinder)
        {
            IPvPRangeCalculator rangeCalculator = _factoryProvider.Targets.RangeCalculatorProvider.SizeInclusiveCalculator;
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
                    EnemyCruiser.BlockedShipsTracker,
                    _targetFactories.HelperFactory.CreateShipRangeHelper(this));
        }

        public void StartMoving()
        {
            // Logging.LogMethod(Tags.SHIPS);
            rigidBody.velocity = new Vector2(maxVelocityInMPerS * _directionMultiplier, 0);
            StartMovementEffects();
        }

        protected virtual void StartMovementEffects() { }

        public void StopMoving()
        {
            // Logging.LogMethod(Tags.SHIPS);
            rigidBody.velocity = new Vector2(0, 0);
            StopMovementEffects();
        }

        protected virtual void StopMovementEffects() { }

        protected override void OnDestroyed()
        {
            CleanUp();
            base.OnDestroyed();
        }

        protected override void ShowDeathEffects()
        {
            _deathPool.GetItem(Position, Faction);
            Deactivate();
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = GetNonTurretRenderers();

            foreach (IPvPBarrelWrapper turret in _turrets)
            {
                renderers.AddRange(turret.Renderers);
            }

            return renderers;
        }

        protected virtual List<SpriteRenderer> GetNonTurretRenderers()
        {
            return base.GetInGameRenderers();
        }

        public void DisableMovement()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            if (_movementDecider != null)
            {
                _movementDecider.DisposeManagedState();
                _movementDecider = null;

                _targetProcessorWrapper.DisposeManagedState();
                _movementTargetProcessor = null;

                _enemyDetectorProvider.DisposeManagedState();
                _enemyDetectorProvider = null;

                _friendDetectorProvider.DisposeManagedState();
                _friendDetectorProvider = null;

                foreach (IPvPBarrelWrapper turret in _turrets)
                {
                    turret.DisposeManagedState();
                }
            }
        }

        public void AddExtraFriendDetectionRange(float extraRange)
        {
            FRIEND_DETECTION_RADIUS_MULTIPLIER += extraRange;
        }
    }
}
