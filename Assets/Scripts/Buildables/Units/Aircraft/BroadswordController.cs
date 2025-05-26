using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Data.Static;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class BroadswordController : AircraftController, ITargetConsumer
    {
        private FollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private IBarrelWrapper _rocketBarrelWrapper, _minigunBarrelWrapper;
        private ITargetProcessor _followingTargetProcessor;
        private ITargetFinder _inRangeTargetFinder;
        private ITargetTracker _inRangeTargetTracker;
        private bool _isAtCruisingHeight;
        private ManualDetectorProvider _hoverTargetDetectorProvider;
        public List<Sprite> allSprites = new List<Sprite>();
        public ManualProximityTargetProcessorWrapper followingTargetProcessorWrapper;

        private const float WITHIN_RANGE_VELOCITY_MULTIPLIER = 0.5f;

        public float enemyHoverRangeInM, enemyFollowRangeInM;

        private ITarget _target;
        public ITarget Target
        {
            get { return _target; }
            set
            {
                Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  {_target?.ToString()} > {value?.ToString()}");

                _target = value;
                _outsideRangeMovementController.Target = _target;
                _inRangeMovementController.Target = _target;

                UpdateMovementController();
            }
        }

        // Expose barrel wrappers to editor
        [SerializeField]
        private List<AircraftBarrelWrapper> barrelWrappers;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
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



        public override void Initialise(UIManager uiManager)
        {
            base.Initialise(uiManager);

            _outsideRangeMovementController = new FollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IVelocityProvider inRangeVelocityProvider
                = new MultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = new FollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
            _isAtCruisingHeight = false;
        }

        protected override void AddBuildRateBoostProviders(
    GlobalBoostProviders globalBoostProviders,
    IList<ObservableCollection<IBoostProvider>> buildRateBoostProvidersList)
        {
            base.AddBuildRateBoostProviders(globalBoostProviders, buildRateBoostProvidersList);
            buildRateBoostProvidersList.Add(_cruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders);
        }

        protected override async void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            SetupTargetDetection();

            foreach (var barrelWrapper in barrelWrappers)
            {
                SoundKey soundKey;
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
                barrelWrapper.Initialise(this, _cruiserSpecificFactories, soundKey);
                barrelWrapper.ApplyVariantStats(this);
            }
            List<Sprite> allSpriteWrappers = new List<Sprite>();
            foreach (Sprite sprite in allSprites)
            {
                allSpriteWrappers.Add(sprite);
            }
            //create Sprite Chooser
            _spriteChooser = new SpriteChooser(allSpriteWrappers, this);
        }

        private void SetupTargetDetection()
        {
            // Create target processor => For following enemies
            TargetProcessorArgs args
                = new TargetProcessorArgs(
                    _cruiserSpecificFactories,
                    FactoryProvider.Targets,
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
                    FactoryProvider.Targets.RangeCalculatorProvider.BasicCalculator);
            ITargetFilter enemyDetectionFilter = new FactionAndTargetTypeFilter(EnemyCruiser.Faction, AttackCapabilities);
            _inRangeTargetFinder = new RangedTargetFinder(_hoverTargetDetectorProvider.TargetDetector, enemyDetectionFilter);
            _inRangeTargetTracker = _cruiserSpecificFactories.Targets.TrackerFactory.CreateTargetTracker(_inRangeTargetFinder);
            _inRangeTargetTracker.TargetsChanged += _hoverRangeTargetTracker_TargetsChanged;
        }

        private void _hoverRangeTargetTracker_TargetsChanged(object sender, EventArgs e)
        {
            Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");
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
                    Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  In range movement controller");
                    return _inRangeMovementController;
                }
                else
                {
                    Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}  Outside of range movement controller");
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
    }
}