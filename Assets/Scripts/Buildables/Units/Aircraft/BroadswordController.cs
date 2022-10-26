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
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.U2D;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class BroadswordController : AircraftController, ITargetConsumer
	{
        private FollowingXAxisMovementController _outsideRangeMovementController, _inRangeMovementController;
        private IBarrelWrapper _barrelWrapper;
        private ITargetProcessor _followingTargetProcessor;
        private ITargetFinder _inRangeTargetFinder;
        private ITargetTracker _inRangeTargetTracker;
		private bool _isAtCruisingHeight;
        private ManualDetectorProvider _hoverTargetDetectorProvider;
        public List<Sprite> allSprites = new List<Sprite>();
        public ManualProximityTargetProcessorWrapper followingTargetProcessorWrapper;

        private const float WITHTIN_RANGE_VELOCITY_MULTIPLIER = 0.5f;

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

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
		{
            base.StaticInitialise(parent, healthBar, commonStrings);

            Assert.IsNotNull(followingTargetProcessorWrapper);

            _barrelWrapper = gameObject.GetComponentInChildren<IBarrelWrapper>();
			Assert.IsNotNull(_barrelWrapper);
			_barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);
		}

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);

            _outsideRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, maxVelocityProvider: this);

            IVelocityProvider inRangeVelocityProvider
                = _movementControllerFactory.CreateMultiplyingVelocityProvider(
                    providerToWrap: this,
                    multiplier: WITHTIN_RANGE_VELOCITY_MULTIPLIER);
            _inRangeMovementController = _movementControllerFactory.CreateFollowingXAxisMovementController(rigidBody, inRangeVelocityProvider);
		}

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
            _isAtCruisingHeight = false;
        }

        protected override async void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            SetupTargetDetection();

            _barrelWrapper.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.BigCannon);
            List<ISpriteWrapper> allSpriteWrappers = new List<ISpriteWrapper>();
            foreach (Sprite sprite in allSprites)
            {
                allSpriteWrappers.Add(new SpriteWrapper(sprite));
            }
            //create Sprite Chooser
            _spriteChooser = new SpriteChooser(new AssignerFactory(), allSpriteWrappers, this);
        }

        private void SetupTargetDetection()
        {
            // Create target processor => For following enemies
            ITargetProcessorArgs args
                = new TargetProcessorArgs(
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
            Logging.Log(Tags.AIRCRAFT, $"{GetInstanceID()}");
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
