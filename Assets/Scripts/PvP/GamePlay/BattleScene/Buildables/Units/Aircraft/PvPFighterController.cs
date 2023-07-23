using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using Unity.Netcode;
using Unity.Netcode.Components;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPFighterController : PvPAircraftController, IPvPTargetConsumer, IPvPTargetProvider
    {
        private IPvPTargetFinder _followableTargetFinder, _shootableTargetFinder;
        private IPvPTargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
        private IPvPExactMatchTargetFilter _exactMatchTargetFilter;
        private IPvPMovementController _figherMovementController;
        private PvPBarrelController _barrelController;
        private IPvPAngleHelper _angleHelper;
        private PvPManualDetectorProvider _followableEnemyDetectorProvider, _shootableEnemeyDetectorProvider;

        public float enemyFollowDetectionRangeInM;

        private const float PATROLLING_VELOCITY_DIVISOR = 2;

        private IPvPTarget _target;
        public IPvPTarget Target
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
                    ActiveMovementController = _figherMovementController;
                }
            }
        }

        protected override float MaxPatrollingVelocity => EffectiveMaxVelocityInMPerS / PATROLLING_VELOCITY_DIVISOR;
        protected override float PositionEqualityMarginInM => 2;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _barrelController = gameObject.GetComponentInChildren<PvPBarrelController>();
            Assert.IsNotNull(_barrelController);
            _barrelController.StaticInitialise();
            AddDamageStats(_barrelController.DamageCapability);
        }

        public override void Initialise( /* IPvPUIManager uiManager,*/ IPvPFactoryProvider factoryProvider)
        {
            base.Initialise( /* uiManager,*/ factoryProvider);
            _angleHelper = _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleHelper();
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);
            _angleHelper = _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleHelper();
        }
        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);

            _figherMovementController
                = _movementControllerFactory.CreateFighterMovementController(
                    rigidBody,
                    maxVelocityProvider: this,
                    targetProvider: this,
                    safeZone: _aircraftProvider.FighterSafeZone);

            // Reset rotation
            Quaternion baseRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = baseRotation;
            rigidBody.rotation = 0;
            // Logging.Verbose(Tags.FIGHTER, $"Id: {GameObject.GetInstanceID()}  After reset rotation: {rigidBody.rotation}");
            OnActivatePvPClientRpc();
        }

        protected async override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                PvPFaction enemyFaction = PvPHelper.GetOppositeFaction(Faction);
                IPvPTarget parent = this;
                IPvPUpdater updater = _factoryProvider.UpdaterProvider.PerFrameUpdater;

                IPvPBarrelControllerArgs args
                    = new PvPBarrelControllerArgs(
                        updater,
                        _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, AttackCapabilities),
                        _factoryProvider.TargetPositionPredictorFactory.CreateLinearPredictor(),
                        _factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleCalculator(),
                        _factoryProvider.Turrets.AttackablePositionFinderFactory.DummyPositionFinder,
                        _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateDummyAdjuster(),
                        _movementControllerFactory.CreateRotationMovementController(_barrelController.pvpTurretStats.TurretRotateSpeedInDegrees, _barrelController.transform, updater),
                        _factoryProvider.Turrets.TargetPositionValidatorFactory.CreateDummyValidator(),
                        _factoryProvider.Turrets.AngleLimiterFactory.CreateFighterLimiter(),
                        _factoryProvider,
                        _cruiserSpecificFactories,
                        parent,
                        _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        _cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        EnemyCruiser,
                        PvPSoundKeys.PvPFiring.BigCannon);

                _barrelController.InitialiseAsync(args);

                SetupTargetDetection();

                _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateFighterSpriteChooserAsync(this);
                OnBuildableCompletedClientRpc();
            }

            if (IsClient)
            {
                OnBuildableCompleted_PvPClient();
                _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateFighterSpriteChooserAsync(this);
            }

        }

        /// <summary>
        /// Enemies first come within following range, and then shootable range as the figher closes
        /// in on the enemy.
        /// 
        /// enemyDetectionRangeInM: 
        ///		The range at which enemies are detected
        /// barrelController.turretStats.rangeInM:  
        ///		The range at which the turret can shoot enemies
        /// enemyDetectionRangeInM > barrelController.turretStats.rangeInM
        /// </summary>
        private void SetupTargetDetection()
        {
            // Detect followable enemies
            _followableEnemyDetectorProvider
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyAircraftTargetDetector(
                    Transform,
                    enemyFollowDetectionRangeInM,
                    _targetFactories.RangeCalculatorProvider.BasicCalculator);
            PvPFaction enemyFaction = PvPHelper.GetOppositeFaction(Faction);
            IList<PvPTargetType> targetTypesToFollow = new List<PvPTargetType>() { PvPTargetType.Aircraft };
            IPvPTargetFilter targetFilter = _targetFactories.FilterFactory.CreateTargetFilter(enemyFaction, targetTypesToFollow);
            _followableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(_followableEnemyDetectorProvider.TargetDetector, targetFilter);

            IPvPTargetRanker followableTargetRanker = _targetFactories.RankerFactory.EqualTargetRanker;
            IPvPRankedTargetTracker followableTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_followableTargetFinder, followableTargetRanker);
            _followableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(followableTargetTracker);
            _followableTargetProcessor.AddTargetConsumer(this);

            // Detect shootable enemies
            _exactMatchTargetFilter = _targetFactories.FilterFactory.CreateMulitpleExactMatchTargetFilter();
            _followableTargetProcessor.AddTargetConsumer(_exactMatchTargetFilter);

            _shootableEnemeyDetectorProvider
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyAircraftTargetDetector(
                    Transform,
                    _barrelController.pvpTurretStats.RangeInM,
                    _targetFactories.RangeCalculatorProvider.BasicCalculator);
            _shootableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(_shootableEnemeyDetectorProvider.TargetDetector, _exactMatchTargetFilter);

            IPvPTargetRanker shootableTargetRanker = _targetFactories.RankerFactory.EqualTargetRanker;
            IPvPRankedTargetTracker shootableTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_shootableTargetFinder, shootableTargetRanker);
            _shootableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(shootableTargetTracker);
            _shootableTargetProcessor.AddTargetConsumer(_barrelController);
        }

        protected override IList<IPvPPatrolPoint> GetPatrolPoints()
        {
            return PvPHelper.ConvertVectorsToPatrolPoints(_aircraftProvider.FindFighterPatrolPoints(cruisingAltitudeInM));
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
                float zRotationInDegrees = _angleHelper.FindAngle(Velocity, transform.IsMirrored());
                Quaternion rotation = rigidBody.transform.rotation;
                rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, zRotationInDegrees);
/*                if (transform.rotation != rotation)
                {
                    Debug.Log(" ===========> calling me now!!!");
                    // sava added for PvP
                    GetComponent<NetworkTransform>()?.Teleport(transform.position, transform.rotation, transform.localScale);                    
                }*/
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

        // BuildableStatus
        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
        }

        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();
        // only for PvPFighter :(
        public NetworkVariable<float> pvp_RotationY = new NetworkVariable<float>();
        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
                if(pvp_RotationY.Value != transform.eulerAngles.y)
                    pvp_RotationY.Value = transform.eulerAngles.y;
            }
            if (IsClient)
            {
                BuildProgress = PvP_BuildProgress.Value;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, pvp_RotationY.Value, transform.eulerAngles.z);
            }
        }

        //------------------------------------ methods for sync, written by Sava ------------------------------//

        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsClient)
                base.OnValueChangedIsEnableRenderes(isEnabled);
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
        }

        // ProgressController Visible
        protected override void CallRpc_ProgressControllerVisible(bool isEnabled)
        {
            OnProgressControllerVisibleClientRpc(isEnabled);
        }

        // set Position of PvPBuildable
        protected override void CallRpc_SetPosition(Vector3 pos)
        {
            //  OnSetPositionClientRpc(pos);
        }

        // Set Rotation of PvPBuildable
        protected override void CallRpc_SetRotation(Quaternion rotation)
        {
            OnSetRotationClientRpc(rotation);
        }
        private void ActiveTrail()
        {
            _aircraftTrailObj.SetActive(true);
        }

        protected override void OnBuildableProgressEvent()
        {
            if (IsClient)
                base.OnBuildableProgressEvent();
            if (IsServer)
                OnBuildableProgressEventClientRpc();
        }
        protected override void OnCompletedBuildableEvent()
        {
            if (IsClient)
                base.OnCompletedBuildableEvent();
            if (IsServer)
                OnCompletedBuildableEventClientRpc();
        }
        protected override void OnDestroyedEvent()
        {
            if (IsClient)
                base.OnDestroyedEvent();
            if (IsServer)
                OnDestroyedEventClientRpc();
        }

        //-------------------------------------- RPCs -------------------------------------------------//

        [ClientRpc]
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            OnValueChangedIsEnableRenderes(isEnabled);
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
            if (!isEnabled)
            {
                Invoke("ActiveTrail", 0.5f);
            }

        }

        [ClientRpc]
        private void OnSetPositionClientRpc(Vector3 pos)
        {
            Position = pos;
        }

        [ClientRpc]
        private void OnSetRotationClientRpc(Quaternion rotation)
        {
            Rotation = rotation;
        }

        [ClientRpc]
        private void OnActivatePvPClientRpc()
        {
            Activate_PvPClient();
        }

        [ClientRpc]
        private void OnBuildableProgressEventClientRpc()
        {
            OnBuildableProgressEvent();
        }


        [ClientRpc]
        private void OnCompletedBuildableEventClientRpc()
        {
            OnCompletedBuildableEvent();
        }

        [ClientRpc]
        private void OnDestroyedEventClientRpc()
        {
            OnDestroyedEvent();
        }

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            OnBuildableCompleted();
        }

        [ClientRpc]
        protected void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            BuildableState = state;
        }
    }
}
