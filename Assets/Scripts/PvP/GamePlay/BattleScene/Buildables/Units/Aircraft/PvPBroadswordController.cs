using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost.GlobalProviders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.UI.Sound;
using BattleCruisers.Data.Static;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Utils;
using BattleCruisers.Targets.TargetFinders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPBroadswordController : PvPAircraftController, ITargetConsumer
    {
        private PvPFollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private IPvPBarrelWrapper _rocketBarrelWrapper, _minigunBarrelWrapper;
        private IPvPTargetProcessor _followingTargetProcessor;
        private ITargetFinder _inRangeTargetFinder;
        private ITargetTracker _inRangeTargetTracker;
        private bool _isAtCruisingHeight;
        private PvPManualDetectorProvider _hoverTargetDetectorProvider;
        public List<Sprite> allSprites = new List<Sprite>();
        public PvPManualProximityTargetProcessorWrapper followingTargetProcessorWrapper;

        private const float WITHIN_RANGE_VELOCITY_MULTIPLIER = 0.5f;

        public float enemyHoverRangeInM, enemyFollowRangeInM;

        private ITarget _target;
        public ITarget Target
        {
            get { return _target; }
            set
            {
                //   Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  {_target?.ToString()} > {value?.ToString()}");

                _target = value;
                _outsideRangeMovementController.Target = _target;
                _inRangeMovementController.Target = _target;

                UpdateMovementController();
            }
        }

        // Expose barrel wrappers to editor
        [SerializeField]
        private List<PvPAircraftBarrelWrapper> barrelWrappers;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            Assert.IsNotNull(followingTargetProcessorWrapper);

            foreach (var barrelWrapper in barrelWrappers)
            {
                Assert.IsNotNull(barrelWrapper);
                barrelWrapper.StaticInitialise();
                AddDamageStats(barrelWrapper.DamageCapability);
            }
        }



        public override void Initialise(/*IPvPUIManager uiManager,*/ IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(/*uiManager,*/ factoryProvider);

            _outsideRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IPvPVelocityProvider inRangeVelocityProvider
                = _movementControllerFactory.CreateMultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);
            _outsideRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IPvPVelocityProvider inRangeVelocityProvider
                = _movementControllerFactory.CreateMultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
        }

        protected override void AddBuildRateBoostProviders(
            IPvPGlobalBoostProviders globalBoostProviders,
            IList<ObservableCollection<IPvPBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            OnActivatePvPClientRpc(activationArgs.ParentCruiser.Position, activationArgs.EnemyCruiser.Position, activationArgs.ParentCruiser.Direction, isAtCruiserHeight: false);
            base.Activate(activationArgs);
            _isAtCruisingHeight = false;
        }

        protected override void OnBuildableCompleted()
        {

            if (IsServer)
            {
                base.OnBuildableCompleted();

                SetupTargetDetection();

                foreach (var barrelWrapper in barrelWrappers)
                {
                    ISoundKey soundKey;
                    switch (barrelWrapper.firingSoundKey)
                    {
                        case "AttackBoat":
                            soundKey = SoundKeys.Firing.AttackBoat;
                            break;
                        case "Missile":
                            soundKey = SoundKeys.Firing.Missile;
                            break;
                        // Add more cases for other sound keys as needed
                        default:
                            soundKey = SoundKeys.Firing.AttackBoat; // default sound key if no match is found
                            break;
                    }
                    barrelWrapper.Initialise(this, _factoryProvider, _cruiserSpecificFactories, soundKey);
                    barrelWrapper.ApplyVariantStats(this);
                }

                List<ISpriteWrapper> allSpriteWrappers = new List<ISpriteWrapper>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(new SpriteWrapper(sprite));
                }
                //create Sprite Chooser
                _spriteChooser = new PvPSpriteChooser(new PvPAssignerFactory(), allSpriteWrappers, this);
            }

            else
            {
                OnBuildableCompleted_PvPClient();
                //barrelWrapper.ApplyVariantStats(this);
                List<ISpriteWrapper> allSpriteWrappers = new List<ISpriteWrapper>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(new SpriteWrapper(sprite));
                }
                //create Sprite Chooser
                _spriteChooser = new PvPSpriteChooser(new PvPAssignerFactory(), allSpriteWrappers, this);
            }

        }

        private void SetupTargetDetection()
        {
            // Create target processor => For following enemies
            IPvPTargetProcessorArgs args
                = new PvPTargetProcessorArgs(
                    _cruiserSpecificFactories,
                    _factoryProvider.Targets,
                    EnemyCruiser.Faction,
                    AttackCapabilities,
                    enemyFollowRangeInM,
                    parentTarget: this);

            _followingTargetProcessor = followingTargetProcessorWrapper.CreateTargetProcessor(args);
            _followingTargetProcessor.AddTargetConsumer(this);

            // Create target tracker => For keeping track of in range targets
            _hoverTargetDetectorProvider
                = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipTargetDetector(
                    Transform,
                    enemyHoverRangeInM,
                    _factoryProvider.Targets.RangeCalculatorProvider.BasicCalculator);
            ITargetFilter enemyDetectionFilter = _factoryProvider.Targets.FilterFactory.CreateTargetFilter(EnemyCruiser.Faction, AttackCapabilities);
            _inRangeTargetFinder = _factoryProvider.Targets.FinderFactory.CreateRangedTargetFinder(_hoverTargetDetectorProvider.TargetDetector, enemyDetectionFilter);
            _inRangeTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(_inRangeTargetFinder);
            _inRangeTargetTracker.TargetsChanged += _hoverRangeTargetTracker_TargetsChanged;
        }

        private void _hoverRangeTargetTracker_TargetsChanged(object sender, EventArgs e)
        {
            //  Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");
            UpdateMovementController();
        }

        protected override IList<IPvPPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.FindDeathstarPatrolPoints(transform.position, cruisingAltitudeInM);
            IList<IPvPPatrolPoint> patrolPoints = new List<IPvPPatrolPoint>(1)
            {
                new PvPPatrolPoint(patrolPositions[1], removeOnceReached: true)
            };
            for (int i = 2; i < patrolPositions.Count; ++i)
            {
                patrolPoints.Add(new PvPPatrolPoint(patrolPositions[i]));
            }
            return patrolPoints;
        }

        private void OnFirstPatrolPointReached()
        {
            _isAtCruisingHeight = true;
            UpdateMovementController();
        }

        private void UpdateMovementController()
        {
            ActiveMovementController = ChooseMovementController();
        }

        private IPvPMovementController ChooseMovementController()
        {
            if (_isAtCruisingHeight && Target != null)
            {
                if (_inRangeTargetTracker.ContainsTarget(Target))
                {
                    //   Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  In range movement controller");
                    return _inRangeMovementController;
                }
                else
                {
                    //   Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  Outside of range movement controller");
                    return _outsideRangeMovementController;
                }
            }
            else
            {
                return PatrollingMovementController;
            }
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            followingTargetProcessorWrapper.DisposeManagedState();
            _followingTargetProcessor = null;

            _inRangeTargetFinder.DisposeManagedState();
            _inRangeTargetFinder = null;

            _inRangeTargetTracker.TargetsChanged -= _hoverRangeTargetTracker_TargetsChanged;
            _inRangeTargetTracker.DisposeManagedState();
            _inRangeTargetTracker = null;

            _hoverTargetDetectorProvider.DisposeManagedState();
            _hoverTargetDetectorProvider = null;

            foreach (var barrelWrapper in barrelWrappers)
            {
                barrelWrapper.DisposeManagedState();
            }
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = base.GetInGameRenderers();

            foreach (var barrelWrapper in barrelWrappers)
            {
                renderers.AddRange(barrelWrapper.Renderers);
            }

            return renderers;
        }

        // sava added

        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();
        // only for PvPFighter :(
        public NetworkVariable<float> pvp_RotationY = new NetworkVariable<float>();

        // BuildableStatus
        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
        }
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
                Rotation = rotation;
        }

        [ClientRpc]
        private void OnActivatePvPClientRpc(Vector3 ParentCruiserPosition, Vector3 EnemyCruiserPosition, Direction facingDirection, bool isAtCruiserHeight)
        {
            if (!IsHost)
            {
                _aircraftProvider = new PvPAircraftProvider(ParentCruiserPosition, EnemyCruiserPosition, RandomGenerator.Instance);
                FacingDirection = facingDirection;
                _isAtCruisingHeight = isAtCruiserHeight;
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