using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Targets.TargetDetectors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BattleCruisers.Utils;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Deciders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using Unity.Netcode;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;

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
    public abstract class PvPShipController : PvPUnit
    {
        private int _directionMultiplier;
        private IList<IPvPBarrelWrapper> turrets;
        private PvPShipTargetProcessorWrapper _targetProcessorWrapper;
        private ITargetProcessor _movementTargetProcessor;
        private IMovementDecider _movementDecider;
        private ManualDetectorProvider _enemyDetectorProvider, _friendDetectorProvider;
        public PvPShipDeathType deathType;
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
        public float OptimalArmamentRangeInM;
        public bool KeepDistanceFromEnemyCruiser;

        private float FriendDetectionRangeInM => FRIEND_DETECTION_RADIUS_MULTIPLIER * Size.x / 2;
        private float EnemyDetectionRangeInM => ENEMY_DETECTION_RADIUS_MULTIPLIER * Size.x / 2;
        public bool IsMoving => rigidBody.velocity.x != 0;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            turrets = transform.GetComponentsInChildren<IPvPBarrelWrapper>();

            foreach (IPvPBarrelWrapper turret in turrets)
            {
                turret.StaticInitialise();
            }

            FindDamageStats();

            _targetProcessorWrapper = transform.FindNamedComponent<PvPShipTargetProcessorWrapper>("ShipTargetProcessorWrapper");
        }

        private void FindDamageStats()
        {
            IList<IDamageCapability> antiAirDamageCapabilities = GetDamageCapabilities(TargetType.Aircraft);
            if (antiAirDamageCapabilities.Count != 0)
            {
                AddDamageStats(new PvPDamageCapability(antiAirDamageCapabilities));
            }

            IList<IDamageCapability> antiSeaDamageCapabilities = GetDamageCapabilities(TargetType.Ships);
            if (antiSeaDamageCapabilities.Count != 0)
            {
                AddDamageStats(new PvPDamageCapability(antiSeaDamageCapabilities));
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

        public override void Initialise()
        {
            base.Initialise();
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                OnShipCompleted();
                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
            }
        }

        protected override void OnBuildableCompleted_PvPClient()
        {
            base.OnBuildableCompleted_PvPClient();
            OnShipCompleted();
        }
        protected virtual void OnShipCompleted()
        {
            if (IsServer)
            {
                InitialiseTurrets();
                SetupMovement();
            }
        }

        // Turrets start attacking targets as soon as they are initialised, so
        // only initialise them once the ship has been completed.
        void InitialiseTurrets()
        {
            foreach(IPvPBarrelWrapper barrelWrapper in turrets)
                barrelWrapper.Initialise(this, _cruiserSpecificFactories);
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
            Faction enemyFaction = PvPHelper.GetOppositeFaction(Faction);

            // Do not want to stop ship from moving if it encounters aircraft
            IList<TargetType> targetProcessorTargetTypes = AttackCapabilities.ToList();
            targetProcessorTargetTypes.Remove(TargetType.Aircraft);
            if (KeepDistanceFromEnemyCruiser)
                targetProcessorTargetTypes.Add(TargetType.Cruiser);

            IPvPTargetProcessorArgs args
                = new PvPTargetProcessorArgs(
                    _cruiserSpecificFactories,
                    enemyFaction,
                    targetProcessorTargetTypes,
                    OptimalArmamentRangeInM,
                    parentTarget: this);

            return _targetProcessorWrapper.CreateTargetProcessor(args);
        }

        private IMovementDecider SetupMovementDecider(ITargetFinder inRangeTargetFinder)
        {
            IRangeCalculator rangeCalculator = PvPTargetFactoriesProvider.RangeCalculatorProvider.SizeInclusiveCalculator;
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
                new PvPShipMovementDecider(
                    this,
                    new PvPShipBlockingEnemyProvider(_enemyDetectorProvider.TargetDetector, this),
                    new PvPShipBlockingFriendlyProvider(_friendDetectorProvider.TargetDetector, this),
                    _cruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(inRangeTargetFinder),
                    EnemyCruiser.BlockedShipsTracker);
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
            if (IsHost)
            {
                PvPPrefabFactory.ShowShipDeath(deathType, Position, Faction);
                Deactivate();
            }
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = GetNonTurretRenderers();

            foreach (IPvPBarrelWrapper turret in turrets)
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

        List<SpriteRenderer> GetNonTurretRenderers()
        {
            return base.GetInGameRenderers();
        }

        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            if (IsServer)
            {
                OnProgressControllerVisibleClientRpc(isEnabled);
                base.CallRpc_ProgressControllerVisible(isEnabled);
            }
            else
                base.CallRpc_ProgressControllerVisible(isEnabled);
        }

        [ClientRpc]
        void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
            if (!IsHost)
                CallRpc_ProgressControllerVisible(isEnabled);
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

                foreach (IPvPBarrelWrapper turret in turrets)
                {
                    turret.DisposeManagedState();
                }
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
                pvp_Health.Value = maxHealth;
        }

        [ClientRpc]
        void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
            foreach(IPvPBarrelWrapper barrelWrapper in turrets)
                barrelWrapper.ApplyVariantStats(this);
        }
    }
}
