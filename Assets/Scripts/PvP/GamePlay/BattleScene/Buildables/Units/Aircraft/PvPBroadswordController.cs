using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders.Filters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPBroadswordController : PvPAircraftController, ITargetConsumer
    {
        private FollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private ITargetProcessor _followingTargetProcessor;
        private ITargetFinder _inRangeTargetFinder;
        private TargetTracker _inRangeTargetTracker;
        private bool _isAtCruisingHeight;
        private ManualDetectorProvider _hoverTargetDetectorProvider;
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

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            Assert.IsNotNull(followingTargetProcessorWrapper);

            foreach (var barrelWrapper in barrelWrappers)
            {
                Assert.IsNotNull(barrelWrapper);
                barrelWrapper.StaticInitialise();
                AddDamageStats(barrelWrapper.DamageCapability);
            }
        }

        public override void Initialise()
        {
            base.Initialise();

            _outsideRangeMovementController = new FollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IVelocityProvider inRangeVelocityProvider
                = new MultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = new FollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
        }

        public override void Initialise(PvPUIManager uiManager)
        {
            base.Initialise(uiManager);
            _outsideRangeMovementController = new FollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IVelocityProvider inRangeVelocityProvider
                = new MultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = new FollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
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
                    barrelWrapper.Initialise(this, _cruiserSpecificFactories);
                    barrelWrapper.ApplyVariantStats(this);
                }

                List<Sprite> allSpriteWrappers = new List<Sprite>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(sprite);
                }
                //create Sprite Chooser
                _spriteChooser = new PvPSpriteChooser(allSpriteWrappers, this);
            }

            else
            {
                OnBuildableCompleted_PvPClient();
                //barrelWrapper.ApplyVariantStats(this);
                List<Sprite> allSpriteWrappers = new List<Sprite>();
                foreach (Sprite sprite in allSprites)
                {
                    allSpriteWrappers.Add(sprite);
                }
                //create Sprite Chooser
                _spriteChooser = new PvPSpriteChooser(allSpriteWrappers, this);
            }

        }

        private void SetupTargetDetection()
        {
            // Create target processor => For following enemies
            IPvPTargetProcessorArgs args
                = new PvPTargetProcessorArgs(
                    _cruiserSpecificFactories,
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
                    PvPTargetFactoriesProvider.RangeCalculatorProvider.BasicCalculator);
            ITargetFilter enemyDetectionFilter = new FactionAndTargetTypeFilter(EnemyCruiser.Faction, AttackCapabilities);
            _inRangeTargetFinder = new RangedTargetFinder(_hoverTargetDetectorProvider.TargetDetector, enemyDetectionFilter);
            _inRangeTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(_inRangeTargetFinder);
            _inRangeTargetTracker.TargetsChanged += _hoverRangeTargetTracker_TargetsChanged;
        }

        private void _hoverRangeTargetTracker_TargetsChanged(object sender, EventArgs e)
        {
            //  Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");
            UpdateMovementController();
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.DeathstarPatrolPoints(transform.position, cruisingAltitudeInM);
            IList<IPatrolPoint> patrolPoints = new List<IPatrolPoint>(1)
            {
                new PatrolPoint(patrolPositions[1], removeOnceReached: true)
            };
            for (int i = 2; i < patrolPositions.Count; ++i)
            {
                patrolPoints.Add(new PatrolPoint(patrolPositions[i]));
            }
            return patrolPoints;
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

        //-------------------------------------- RPCs -------------------------------------------------//
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
    }
}