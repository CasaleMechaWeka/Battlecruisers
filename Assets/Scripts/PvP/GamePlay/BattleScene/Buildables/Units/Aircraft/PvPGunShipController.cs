using BattleCruisers.Buildables;
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
using BattleCruisers.Targets.TargetTrackers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPGunShipController : PvPAircraftController, ITargetConsumer
    {
        private FollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private IPvPBarrelWrapper _barrelWrapper;
        private ITargetProcessor _followingTargetProcessor;
        private ITargetFinder _inRangeTargetFinder;
        private TargetTracker _inRangeTargetTracker;
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

        public override void Initialise()
        {
            base.Initialise();

            _outsideRangeMovementController = new FollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IVelocityProvider inRangeVelocityProvider
                = new MultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHTIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = new FollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
        }

        public override void Initialise(PvPUIManager uiManager)
        {
            base.Initialise(uiManager);

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

        protected override async void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                SetupTargetDetection();

                _barrelWrapper.Initialise(this, _cruiserSpecificFactories);
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
            // Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");
            UpdateMovementController();
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.GunshipPatrolPoints(cruisingAltitudeInM);
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
    }
}
