using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
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
    public class ShipController : Unit
    {
        private int _directionMultiplier;
        protected IList<IBarrelWrapper> turrets;
        private ShipTargetProcessorWrapper _targetProcessorWrapper;
        private ITargetProcessor _movementTargetProcessor;
        private IMovementDecider _movementDecider;
        private ManualDetectorProvider _enemyDetectorProvider, _friendDetectorProvider;
        public float OptimalArmamentRangeInM = -1;
        public ShipDeathType deathType;

        const float FRIEND_DETECTION_RADIUS_MULTIPLIER = 1.2f;
        private const float ENEMY_DETECTION_RADIUS_MULTIPLIER = 2;

        public override TargetType TargetType => TargetType.Ships;

        [SerializeField]
        private float ySpawnOffset = -0.35f; // Default value, can be adjusted in the Inspector

        public override float YSpawnOffset => ySpawnOffset;
        

        [SerializeField]
        private List<GameObject> additionalRenderers = new List<GameObject>();


        /// <summary>
        /// Optimal range for ship to do the most damage, while staying out of
        /// range of defence buildings.
        /// 
        /// Usually this will simply be the range of the ship's longest ranged turret,
        /// but can be less if we want multiple of the ships turrets to be in range.
        /// </summary>
        public bool KeepDistanceFromEnemyCruiser;

        private float FriendDetectionRangeInM => FRIEND_DETECTION_RADIUS_MULTIPLIER * Size.x / 2;
        private float EnemyDetectionRangeInM => ENEMY_DETECTION_RADIUS_MULTIPLIER * Size.x / 2;
        public bool IsMoving => rigidBody.velocity.x != 0;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            turrets = gameObject.GetComponentsInChildren<IBarrelWrapper>();

            foreach (IBarrelWrapper turret in turrets)
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
                turrets
                    .Where(turret => turret.DamageCapability.AttackCapabilities.Contains(attackCapability))
                    .Select(turret => turret.DamageCapability)
                    .ToList();
        }

        public override void Initialise(UIManager uiManager)
        {
            base.Initialise(uiManager);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            OnShipCompleted();
            foreach(IBarrelWrapper barrelWrapper in turrets)
                barrelWrapper.ApplyVariantStats(this);
        }

        protected virtual void OnShipCompleted()
        {
            foreach(IBarrelWrapper barrelWrapper in turrets)
                barrelWrapper.Initialise(this, _cruiserSpecificFactories);
            SetupMovement();
        }


        protected void SetupMovement()
        {
            _directionMultiplier = FacingDirection == Direction.Right ? 1 : -1;
            _movementTargetProcessor = SetupTargetProcessorWrapper();
            _movementDecider = SetupMovementDecider(_targetProcessorWrapper.InRangeTargetFinder);
            _movementTargetProcessor.AddTargetConsumer(_movementDecider);
        }

        private ITargetProcessor SetupTargetProcessorWrapper()
        {
            Faction enemyFaction = Helper.GetOppositeFaction(Faction);

            // Do not want to stop ship from moving if it encounters aircraft
            IList<TargetType> targetProcessorTargetTypes = AttackCapabilities.ToList();
            targetProcessorTargetTypes.Remove(TargetType.Aircraft);
            if (KeepDistanceFromEnemyCruiser)
                targetProcessorTargetTypes.Add(TargetType.Cruiser);

            if(OptimalArmamentRangeInM == -1)
            {
                Debug.LogError("Field OptimalArmamentRangeInM is not set. Set to >= 0 to confirm it is set");
                OptimalArmamentRangeInM = 0;
            }

            TargetProcessorArgs args
                = new TargetProcessorArgs(
                    _cruiserSpecificFactories,
                    FactoryProvider.Targets,
                    enemyFaction,
                    targetProcessorTargetTypes,
                    OptimalArmamentRangeInM,
                    parentTarget: this);

            return _targetProcessorWrapper.CreateTargetProcessor(args);
        }

        private IMovementDecider SetupMovementDecider(ITargetFinder inRangeTargetFinder)
        {
            IRangeCalculator rangeCalculator = FactoryProvider.Targets.RangeCalculatorProvider.SizeInclusiveCalculator;
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
                new ShipMovementDecider(
                    this,
                    _cruiserSpecificFactories.Targets.ProviderFactory.CreateShipBlockingEnemyProvider(_enemyDetectorProvider.TargetDetector, this),
                    _cruiserSpecificFactories.Targets.ProviderFactory.CreateShipBlockingFriendlyProvider(_friendDetectorProvider.TargetDetector, this),
                    _cruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(inRangeTargetFinder),
                    EnemyCruiser.BlockedShipsTracker,
                    KeepDistanceFromEnemyCruiser ? OptimalArmamentRangeInM : 0);
        }

        public void StartMoving()
        {
            Logging.LogMethod(Tags.SHIPS);
            rigidBody.velocity = new Vector2(maxVelocityInMPerS * _directionMultiplier, 0);
            StartMovementEffects();
        }

        protected virtual void StartMovementEffects() { }

        public virtual void StopMoving()
        {
            Logging.LogMethod(Tags.SHIPS);
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
            PrefabFactory.ShowShipDeath(deathType, Position, Faction);
            Deactivate();
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = base.GetInGameRenderers();

            foreach (IBarrelWrapper turret in turrets)
            {
                renderers.AddRange(turret.Renderers);
            }

            foreach (GameObject obj in additionalRenderers)
            {
                if (obj != null)
                {
                    SpriteRenderer[] spriteRenderers = obj.GetComponentsInChildren<SpriteRenderer>();
                    renderers.AddRange(spriteRenderers);
                }
            }

            return renderers;
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

                foreach (IBarrelWrapper turret in turrets)
                {
                    turret.DisposeManagedState();
                }
            }
        }
    }
}
