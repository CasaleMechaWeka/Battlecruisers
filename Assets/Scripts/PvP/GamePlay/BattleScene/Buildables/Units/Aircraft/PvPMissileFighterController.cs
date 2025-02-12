using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Data.Static;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPMissileFighterController : PvPAircraftController, ITargetConsumer, IPvPTargetProvider
    {
        private ITargetFinder _followableTargetFinder, _shootableTargetFinder;
        private IPvPTargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
        private IPvPMovementController _fighterMovementController;
        private IAngleHelper _angleHelper;
        private PvPManualDetectorProvider _followableEnemyDetectorProvider, _shootableEnemyDetectorProvider;
        private PvPAircraftBarrelWrapper barrelWrapper;
        private PvPBarrelController[] _barrelControllers;

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

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            _barrelControllers = gameObject.GetComponentsInChildren<PvPBarrelController>();
            barrelWrapper = gameObject.GetComponentInChildren<PvPAircraftBarrelWrapper>();
            Assert.IsNotNull(_barrelControllers);
            foreach (var controller in _barrelControllers)
            {
                controller.StaticInitialise();
            }
            Assert.IsNotNull(barrelWrapper);
            barrelWrapper.StaticInitialise();
            AddDamageStats(barrelWrapper.DamageCapability);
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
            OnActivatePvPClientRpc(activationArgs.ParentCruiser.Position, activationArgs.EnemyCruiser.Position, activationArgs.ParentCruiser.Direction, isAtCruiserHeight: false);
            base.Activate(activationArgs);
            _fighterMovementController
                = _movementControllerFactory.CreateFighterMovementController(
                    rigidBody,
                    maxVelocityProvider: this,
                    targetProvider: this,
                    safeZone: _aircraftProvider.FighterSafeZone);

            // Reset rotation
            transform.rotation = Quaternion.identity;
            rigidBody.rotation = 0;
            // Logging.Verbose(Tags.FIGHTER, $"Id: {GameObject.GetInstanceID()}  After reset rotation: {rigidBody.rotation}");
        }

        protected async override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                barrelWrapper.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.Missile);
                barrelWrapper.ApplyVariantStats(this);

                SetupTargetDetection();

                _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateAircraftSpriteChooserAsync(BattleCruisers.Utils.PrefabKeyName.Unit_MissileFighter, this);

                foreach (var controller in _barrelControllers)
                {
                    controller.ApplyVariantStats(this);
                }

                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();

                _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateAircraftSpriteChooserAsync(BattleCruisers.Utils.PrefabKeyName.Unit_MissileFighter, this);

                foreach (var controller in _barrelControllers)
                {
                    controller.ApplyVariantStats(this);
                }
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
            // Detect followable targets
            _followableEnemyDetectorProvider = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                Transform, enemyFollowDetectionRangeInM, _targetFactories.RangeCalculatorProvider.BasicCalculator);

            ITargetFilter followTargetFilter = _targetFactories.FilterFactory.CreateTargetFilter(
                PvPHelper.GetOppositeFaction(Faction), new List<TargetType> { TargetType.Aircraft, TargetType.Ships });

            _followableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(
                _followableEnemyDetectorProvider.TargetDetector, followTargetFilter);

            _followableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(
                _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_followableTargetFinder, _targetFactories.RankerFactory.EqualTargetRanker));

            _followableTargetProcessor.AddTargetConsumer(this);

            // Detect shootable targets
            _shootableEnemyDetectorProvider = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                Transform, Mathf.Max(_barrelControllers[0].pvpTurretStats.RangeInM), _targetFactories.RangeCalculatorProvider.BasicCalculator);

            ITargetFilter shootableTargetFilter = _targetFactories.FilterFactory.CreateMulitpleExactMatchTargetFilter();

            _shootableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(
                _shootableEnemyDetectorProvider.TargetDetector, shootableTargetFilter);

            _shootableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(
                _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_shootableTargetFinder, _targetFactories.RankerFactory.EqualTargetRanker));

            foreach (var controller in _barrelControllers)
            {
                _shootableTargetProcessor.AddTargetConsumer(controller);
            }
        }

        protected override IList<IPvPPatrolPoint> GetPatrolPoints()
        {
            return PvPHelper.ConvertVectorsToPatrolPoints(_aircraftProvider.FindMissileFighterPatrolPoints(cruisingAltitudeInM));
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            FaceVelocityDirection();
        }

        private void FaceVelocityDirection()
        {
            if (Velocity != Vector2.zero)
            {
                float zRotationInDegrees = _angleHelper.FindAngle(Velocity, transform.IsMirrored());
                Quaternion rotation = rigidBody.transform.rotation;
                rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, zRotationInDegrees);
                transform.rotation = rotation;
            }
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _followableEnemyDetectorProvider?.DisposeManagedState();
            _followableTargetProcessor?.DisposeManagedState();
            _shootableEnemyDetectorProvider?.DisposeManagedState();
            _shootableTargetProcessor?.DisposeManagedState();

            // Do not set to null, only created once in StaticInitialise(), so reused by unit pools.
            foreach (var barrel in _barrelControllers)
            {
                barrel.CleanUp();
            }
        }

        // BuildableStatus
        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
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

        //------------------------------------ methods for sync, written by Sava ------------------------------//

        // Visibility 
        protected override void OnValueChangedIsEnableRenderes(bool isEnabled)
        {
            if (IsServer)
                OnValueChangedIsEnabledRendersClientRpc(isEnabled);
            else
                base.OnValueChangedIsEnableRenderes(isEnabled);
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
        private void OnValueChangedIsEnabledRendersClientRpc(bool isEnabled)
        {
            if (!IsHost)
                OnValueChangedIsEnableRenderes(isEnabled);
        }

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
        private void OnSetPositionClientRpc(Vector3 pos)
        {
            if (!IsHost)
                Position = pos;
        }

        [ClientRpc]
        private void OnSetRotationClientRpc(Quaternion rotation)
        {
            if (!IsHost)
            {
                Rotation = rotation;
            }

        }

        [ClientRpc]
        private void OnActivatePvPClientRpc(Vector3 ParentCruiserPosition, Vector3 EnemyCruiserPosition, PvPDirection facingDirection, bool isAtCruiserHeight)
        {
            if (!IsHost)
            {
                _aircraftProvider = new PvPAircraftProvider(ParentCruiserPosition, EnemyCruiserPosition, RandomGenerator.Instance);
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

        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted();
        }

        [ClientRpc]
        protected void OnBuildableStateValueChangedClientRpc(PvPBuildableState state)
        {
            if (!IsHost)
                BuildableState = state;
        }
    }
}
