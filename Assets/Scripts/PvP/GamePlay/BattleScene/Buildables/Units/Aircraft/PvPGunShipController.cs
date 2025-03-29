using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetTrackers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Buildables.Units.Aircraft;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPGunShipController : PvPAircraftController, ITargetConsumer
    {
        private FollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private IPvPBarrelWrapper _barrelWrapper;
        private ITargetProcessor _followingTargetProcessor;
        private ITargetFinder _inRangeTargetFinder;
        private ITargetTracker _inRangeTargetTracker;
        private bool _isAtCruisingHeight;
        private ManualDetectorProvider _hoverTargetDetectorProvider;
        public List<Sprite> allSprites = new List<Sprite>();
        public PvPManualProximityTargetProcessorWrapper followingTargetProcessorWrapper;

        private const float WITHTIN_RANGE_VELOCITY_MULTIPLIER = 0.5f;

        public float enemyHoverRangeInM, enemyFollowRangeInM;

        private ITarget _target;
        public ITarget Target
        {
            get { return _target; }
            set
            {
                // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  {_target?.ToString()} > {value?.ToString()}");

                _target = value;
                _outsideRangeMovementController.Target = _target;
                _inRangeMovementController.Target = _target;

                UpdateMovementController();
            }
        }

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            Assert.IsNotNull(followingTargetProcessorWrapper);

            _barrelWrapper = gameObject.GetComponentInChildren<IPvPBarrelWrapper>();
            Assert.IsNotNull(_barrelWrapper);
            _barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);
        }

        public override void Initialise( /* IPvPUIManager uiManager,*/ IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(/* uiManager,*/ factoryProvider);

            _outsideRangeMovementController = new FollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IVelocityProvider inRangeVelocityProvider
                = new MultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHTIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = new FollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);

            _outsideRangeMovementController = new FollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IVelocityProvider inRangeVelocityProvider
                = new MultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHTIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = new FollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
        }

        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            OnActivatePvPClientRpc(activationArgs.ParentCruiser.Position, activationArgs.EnemyCruiser.Position, activationArgs.ParentCruiser.Direction, isAtCruiserHeight: false);
            base.Activate(activationArgs);
            _isAtCruisingHeight = false;
        }

        public override void Activate_PvPClient()
        {
            base.Activate_PvPClient();
        }

        protected override async void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                SetupTargetDetection();

                _barrelWrapper.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.BigCannon);
                List<Sprite> allSpriteWrappers = new List<Sprite>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(sprite);
                }
                //create Sprite Chooser
                _spriteChooser = new PvPSpriteChooser(allSpriteWrappers, this);
                _barrelWrapper.ApplyVariantStats(this);

                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
                List<Sprite> allSpriteWrappers = new List<Sprite>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(sprite);
                }
                //create Sprite Chooser
                _spriteChooser = new PvPSpriteChooser(allSpriteWrappers, this);
                _barrelWrapper.ApplyVariantStats(this);
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
            _inRangeTargetFinder = new RangedTargetFinder(_hoverTargetDetectorProvider.TargetDetector, enemyDetectionFilter);
            _inRangeTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(_inRangeTargetFinder);
            _inRangeTargetTracker.TargetsChanged += _hoverRangeTargetTracker_TargetsChanged;
        }

        private void _hoverRangeTargetTracker_TargetsChanged(object sender, EventArgs e)
        {
            // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");
            UpdateMovementController();
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.FindGunshipPatrolPoints(cruisingAltitudeInM);
            return ProcessPatrolPoints(patrolPositions, OnFirstPatrolPointReached);
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

        private IMovementController ChooseMovementController()
        {
            if (_isAtCruisingHeight && Target != null)
            {
                if (_inRangeTargetTracker.ContainsTarget(Target))
                {
                    // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  In range movement controller");
                    return _inRangeMovementController;
                }
                else
                {
                    // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  Outside of range movement controller");
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

            _barrelWrapper.DisposeManagedState();
        }

        protected override List<SpriteRenderer> GetInGameRenderers()
        {
            List<SpriteRenderer> renderers = base.GetInGameRenderers();
            renderers.AddRange(_barrelWrapper.Renderers);
            return renderers;
        }

        // BuildableStatus
        protected override void OnBuildableStateValueChanged(PvPBuildableState state)
        {
            OnBuildableStateValueChangedClientRpc(state);
        }
        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();
        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
            }
            else
            {
                BuildProgress = PvP_BuildProgress.Value;

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
                _aircraftProvider = new AircraftProvider(ParentCruiserPosition, EnemyCruiserPosition);
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
