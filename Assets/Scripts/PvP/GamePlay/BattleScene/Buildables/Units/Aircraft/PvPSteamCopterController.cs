using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.U2D;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPSteamCopterController : PvPAircraftController, IPvPTargetConsumer
    {
        private PvPFollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private IPvPBarrelWrapper _barrelWrapper;
        private IPvPTargetProcessor _followingTargetProcessor;
        private IPvPTargetFinder _inRangeTargetFinder;
        private IPvPTargetTracker _inRangeTargetTracker;
        private bool _isAtCruisingHeight;
        private PvPManualDetectorProvider _hoverTargetDetectorProvider;
        public List<Sprite> allSprites = new List<Sprite>();

        public PvPManualProximityTargetProcessorWrapper followingTargetProcessorWrapper;

        private const float WITHTIN_RANGE_VELOCITY_MULTIPLIER = 0.5f;

        public float enemyHoverRangeInM, enemyFollowRangeInM;

        private IPvPTarget _target;
        public IPvPTarget Target
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

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            Assert.IsNotNull(followingTargetProcessorWrapper);

            _barrelWrapper = gameObject.GetComponentInChildren<IPvPBarrelWrapper>();
            Assert.IsNotNull(_barrelWrapper);
            _barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);
        }

        public override void Initialise(/* IPvPUIManager uiManager,*/ IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(/* uiManager, */ factoryProvider);

            _outsideRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IPvPVelocityProvider inRangeVelocityProvider
                = _movementControllerFactory.CreateMultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHTIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
        }

        public override void Initialise(IPvPFactoryProvider factoryProvider, IPvPUIManager uiManager)
        {
            base.Initialise(factoryProvider, uiManager);
            _outsideRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IPvPVelocityProvider inRangeVelocityProvider
                = _movementControllerFactory.CreateMultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHTIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
        }


        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
            _isAtCruisingHeight = false;

            OnActivatePvPClientRpc();
        }

        protected override async void OnBuildableCompleted()
        {

            if(IsServer)
            {
                base.OnBuildableCompleted();

                SetupTargetDetection();

                _barrelWrapper.Initialise(this, _factoryProvider, _cruiserSpecificFactories, PvPSoundKeys.PvPFiring.BigCannon);

                List<IPvPSpriteWrapper> allSpriteWrappers = new List<IPvPSpriteWrapper>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(new PvPSpriteWrapper(sprite));
                }
                //create Sprite Chooser
                _spriteChooser = new PvPSpriteChooser(new PvPAssignerFactory(), allSpriteWrappers, this);

                OnBuildableCompletedClientRpc();
            }
            if(IsClient)
            {
                OnBuildableCompleted_PvPClient();

                List<IPvPSpriteWrapper> allSpriteWrappers = new List<IPvPSpriteWrapper>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(new PvPSpriteWrapper(sprite));
                }
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
            IPvPTargetFilter enemyDetectionFilter = _factoryProvider.Targets.FilterFactory.CreateTargetFilter(EnemyCruiser.Faction, AttackCapabilities);
            _inRangeTargetFinder = _factoryProvider.Targets.FinderFactory.CreateRangedTargetFinder(_hoverTargetDetectorProvider.TargetDetector, enemyDetectionFilter);
            _inRangeTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(_inRangeTargetFinder);
            _inRangeTargetTracker.TargetsChanged += _hoverRangeTargetTracker_TargetsChanged;
        }

        private void _hoverRangeTargetTracker_TargetsChanged(object sender, EventArgs e)
        {
            // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");
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

        private void OnClearingLaunchStation()
        {
            // Stop moving
            //ActiveMovementController = DummyMovementController;
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
