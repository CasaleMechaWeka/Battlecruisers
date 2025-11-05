using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Movement.Velocity.Homing;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPMissileFighterController : PvPAircraftController, ITargetConsumer, ITargetProvider
    {
        private ITargetFinder _followableTargetFinder, _shootableTargetFinder;
        private ITargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
        private IExactMatchTargetFilter _exactMatchTargetFilter;
        private IMovementController _fighterMovementController;
        private PvPBarrelController _barrelController;
        private ManualDetectorProvider _followableEnemyDetectorProvider, _shootableEnemeyDetectorProvider;

        public float enemyFollowDetectionRangeInM;

        private const float PATROLLING_VELOCITY_DIVISOR = 2;

        private ITarget _target;
        public ITarget Target
        {
            get { return _target; }
            set
            {
                // Logging.Log(Tags.FIGHTER, string.Empty + value);

                _target = value;

                if (_target == null)
                {
                    ActiveMovementController = PatrollingMovementController;
                }
                else
                {
                    ActiveMovementController = _fighterMovementController;
                }
            }
        }

        protected override float MaxPatrollingVelocity => EffectiveMaxVelocityInMPerS / PATROLLING_VELOCITY_DIVISOR;
        protected override float PositionEqualityMarginInM => 2;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            _barrelController = gameObject.GetComponentInChildren<PvPBarrelController>();
            Assert.IsNotNull(_barrelController);
            _barrelController.StaticInitialise();
            AddDamageStats(_barrelController.DamageCapability);
        }

        public override void Initialise()
        {
            base.Initialise();
        }

        public override void Initialise(PvPUIManager uiManager)
        {
            base.Initialise(uiManager);
        }
        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            OnActivatePvPClientRpc(activationArgs.ParentCruiser.Position, activationArgs.EnemyCruiser.Position, activationArgs.ParentCruiser.Direction, isAtCruiserHeight: false);
            base.Activate(activationArgs);
            _fighterMovementController
                = new FighterMovementController(
                    rigidBody,
                    maxVelocityProvider: this,
                    targetProvider: this,
                    safeZone: _aircraftProvider.FighterSafeZone);

            // Reset rotation
            Quaternion baseRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = baseRotation;
            rigidBody.rotation = 0;
            // Logging.Verbose(Tags.FIGHTER, $"Id: {GameObject.GetInstanceID()}  After reset rotation: {rigidBody.rotation}");
        }

        protected async override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                Faction enemyFaction = PvPHelper.GetOppositeFaction(Faction);
                ITarget parent = this;
                IUpdater updater = PvPFactoryProvider.UpdaterProvider.PerFrameUpdater;

                IPvPBarrelControllerArgs args
                    = new PvPBarrelControllerArgs(
                        updater,
                        new FactionAndTargetTypeFilter(enemyFaction, AttackCapabilities),
                        new LinearTargetPositionPredictor(),
                        new AngleCalculator(),
                        new AccuracyAdjuster((0, 0)),
                        new RotationMovementController(new TransformBC(_barrelController.transform), updater, _barrelController.TurretStats.TurretRotateSpeedInDegrees),
                        new FacingMinRangePositionValidator(0, true),
                        new AngleLimiter(-180, 180),
                        _cruiserSpecificFactories,
                        parent,
                        _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        EnemyCruiser,
                        SoundKeys.Firing.BigCannon);

                _ = _barrelController.InitialiseAsync(args);

                SetupTargetDetection();

                IList<Sprite> aircraftSprites = await SpriteProvider.GetAircraftSpritesAsync(PrefabKeyName.Unit_MissileFighter);
                _spriteChooser = new PvPSpriteChooser(aircraftSprites, this);
                _barrelController.ApplyVariantStats(this);
                OnBuildableCompletedClientRpc();
            }

            else
            {
                OnBuildableCompleted_PvPClient();
                _barrelController.ApplyVariantStats(this);
                IList<Sprite> aircraftSprites = await SpriteProvider.GetAircraftSpritesAsync(PrefabKeyName.Unit_MissileFighter);
                _spriteChooser = new PvPSpriteChooser(aircraftSprites, this);
            }

        }

        /// <summary>
        /// Enemies first come within following range, and then shootable range as the figher closes
        /// in on the enemy.
        /// 
        /// enemyFollowDetectionRangeInM: 
        ///		The range at which enemies are detected
        /// barrelController.turretStats.rangeInM:  
        ///		The range at which the turret can shoot enemies
        /// enemyFollowDetectionRangeInM > barrelController.turretStats.rangeInM
        /// </summary>
        private void SetupTargetDetection()
        {
            // Detect followable enemies
            _followableEnemyDetectorProvider
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                    Transform,
                    enemyFollowDetectionRangeInM,
                    PvPTargetFactoriesProvider.RangeCalculatorProvider.BasicCalculator);
            Faction enemyFaction = PvPHelper.GetOppositeFaction(Faction);
            IList<TargetType> targetTypesToFollow = new List<TargetType>() { TargetType.Aircraft, TargetType.Ships };
            ITargetFilter targetFilter = new FactionAndTargetTypeFilter(enemyFaction, targetTypesToFollow);
            _followableTargetFinder = new RangedTargetFinder(_followableEnemyDetectorProvider.TargetDetector, targetFilter);

            ITargetRanker followableTargetRanker = PvPTargetFactoriesProvider.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker followableTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_followableTargetFinder, followableTargetRanker);
            _followableTargetProcessor = new TargetProcessor(followableTargetTracker);
            _followableTargetProcessor.AddTargetConsumer(this);

            // Detect shootable enemies
            _exactMatchTargetFilter = new MultipleExactMatchesTargetFilter();
            _followableTargetProcessor.AddTargetConsumer(_exactMatchTargetFilter);

            _shootableEnemeyDetectorProvider
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                    Transform,
                    _barrelController.TurretStats.RangeInM,
                    PvPTargetFactoriesProvider.RangeCalculatorProvider.BasicCalculator);
            _shootableTargetFinder = new RangedTargetFinder(_shootableEnemeyDetectorProvider.TargetDetector, _exactMatchTargetFilter);

            ITargetRanker shootableTargetRanker = PvPTargetFactoriesProvider.RankerFactory.EqualTargetRanker;
            IRankedTargetTracker shootableTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_shootableTargetFinder, shootableTargetRanker);
            _shootableTargetProcessor = new TargetProcessor(shootableTargetTracker);
            _shootableTargetProcessor.AddTargetConsumer(_barrelController);
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            return PvPHelper.ConvertVectorsToPatrolPoints(_aircraftProvider.FighterPatrolPoints(cruisingAltitudeInM));
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            FaceVelocityDirection();
        }

        private void FaceVelocityDirection()
        {
            // Logging.Verbose(Tags.FIGHTER, $"Id: {GameObject.GetInstanceID()}  Velocity: {Velocity}");

            if (Velocity != Vector2.zero)
            {
                float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
                float zRotationInDegrees = transform.IsMirrored() ? 180 - angle : (angle + 360) % 360;

                Quaternion rotation = rigidBody.transform.rotation;
                rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, zRotationInDegrees);
                transform.rotation = rotation;
            }
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _followableEnemyDetectorProvider.DisposeManagedState();
            _followableEnemyDetectorProvider = null;

            _followableTargetProcessor.RemoveTargetConsumer(this);
            _followableTargetProcessor.RemoveTargetConsumer(_exactMatchTargetFilter);
            _followableTargetProcessor.DisposeManagedState();
            _followableTargetProcessor = null;

            _followableTargetFinder.DisposeManagedState();
            _followableTargetFinder = null;

            _shootableEnemeyDetectorProvider.DisposeManagedState();
            _shootableEnemeyDetectorProvider = null;

            _shootableTargetProcessor.RemoveTargetConsumer(_barrelController);
            _shootableTargetProcessor.DisposeManagedState();
            _shootableTargetProcessor = null;

            _shootableTargetFinder.DisposeManagedState();
            _shootableTargetFinder = null;

            // Do not set to null, only created once in StaticInitialise(), so reused by unit pools.
            _barrelController.CleanUp();
        }



        // sava added
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();
        // only for PvPFighter :(
        public NetworkVariable<float> pvp_RotationY = new NetworkVariable<float>();
        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
                if (pvp_RotationY.Value != transform.eulerAngles.y)
                    pvp_RotationY.Value = transform.eulerAngles.y;
            }
            else
            {
                BuildProgress = PvP_BuildProgress.Value;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, pvp_RotationY.Value, transform.eulerAngles.z);
            }
        }

        // ProgressController Visible
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


        private void ActiveTrail()
        {
            _aircraftTrailObj.SetActive(true);
        }

        protected override void OnBuildableProgressEvent()
        {
            if (IsServer)
                OnBuildableProgressEventClientRpc();
            else
                base.OnBuildableProgressEvent();
        }
        protected override void OnCompletedBuildableEvent()
        {
            if (IsServer)
                OnCompletedBuildableEventClientRpc();
            else
                base.OnCompletedBuildableEvent();
        }
        protected override void OnDestroyedEvent()
        {
            if (IsServer)
                OnDestroyedEventClientRpc();
            else
                base.OnDestroyedEvent();
        }

        //-------------------------------------- RPCs -------------------------------------------------//

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
            if (!IsHost)
            {
                if (!isEnabled)
                {
                    Invoke("ActiveTrail", 0.5f);
                }
            }
        }

        [ClientRpc]
        private void OnActivatePvPClientRpc(Vector3 ParentCruiserPosition, Vector3 EnemyCruiserPosition, Direction facingDirection, bool isAtCruiserHeight)
        {
            if (!IsHost)
            {
                _aircraftProvider = new AircraftProvider(ParentCruiserPosition, EnemyCruiserPosition);
                FacingDirection = facingDirection;
                //    _isAtCruisingHeight = isAtCruiserHeight;
                Activate_PvPClient();
            }
        }

        [ClientRpc]
        private void OnBuildableProgressEventClientRpc()
        {
            if (!IsHost)
                OnBuildableProgressEvent();
        }


        [ClientRpc]
        private void OnCompletedBuildableEventClientRpc()
        {
            if (!IsHost)
                OnCompletedBuildableEvent();
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
                OnDestroyedEvent();
        }



    }
}
