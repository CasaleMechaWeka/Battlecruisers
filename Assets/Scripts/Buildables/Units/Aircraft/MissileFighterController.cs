using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Targets;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Utils;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Buildables.Pools;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class MissileFighterController : AircraftController, ITargetConsumer, ITargetProvider
    {
        private ITargetFinder _followableTargetFinder, _shootableTargetFinder;
        private ITargetProcessor _followableTargetProcessor, _shootableTargetProcessor;
        private IMovementController _fighterMovementController;
        private IAngleHelper _angleHelper;
        private ManualDetectorProvider _followableEnemyDetectorProvider, _shootableEnemyDetectorProvider;
        private AircraftBarrelWrapper barrelWrapper;
        private BarrelController[] _barrelControllers;
        public float enemyFollowDetectionRangeInM;
        private const float AngleTolerance = 270f;
        private const float PATROLLING_VELOCITY_DIVISOR = 2;

        private ITarget _target;
        public ITarget Target
        {
            get { return _target; }
            set
            {
                _target = value;
                ActiveMovementController = (_target == null) ? PatrollingMovementController : _fighterMovementController;
            }
        }

        protected override float MaxPatrollingVelocity => EffectiveMaxVelocityInMPerS / PATROLLING_VELOCITY_DIVISOR;
        protected override float PositionEqualityMarginInM => 2;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            // Initialize barrel controllers and barrelWrapper
            _barrelControllers = gameObject.GetComponentsInChildren<BarrelController>();
            barrelWrapper = gameObject.GetComponentInChildren<AircraftBarrelWrapper>();
            Assert.IsNotNull(_barrelControllers);
            foreach (var controller in _barrelControllers)
            {
                controller.StaticInitialise();
            }
            Assert.IsNotNull(barrelWrapper);
            barrelWrapper.StaticInitialise();
            AddDamageStats(barrelWrapper.DamageCapability);
        }

        public override void Initialise(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);
            _angleHelper = factoryProvider.Turrets.AngleCalculatorFactory.CreateAngleHelper();
        }

        public override void Activate(BuildableActivationArgs activationArgs)
        {
            base.Activate(activationArgs);
            _fighterMovementController = _movementControllerFactory.CreateFighterMovementController(
                rigidBody, maxVelocityProvider: this, targetProvider: this, safeZone: _aircraftProvider.FighterSafeZone);
            transform.rotation = Quaternion.identity;
            rigidBody.rotation = 0;
        }

        protected async override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            barrelWrapper.Initialise(this, _factoryProvider, _cruiserSpecificFactories, SoundKeys.Firing.Missile);
            barrelWrapper.ApplyVariantStats(this);
            SetupTargetDetection();

            _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateAircraftSpriteChooserAsync(PrefabKeyName.Unit_MissileFighter, this);
            for (int i = 0; i < _barrelControllers.Length; i++)
            {
                _barrelControllers[i].ApplyVariantStats(this);
            }
        }

        private void SetupTargetDetection()
        {
            // Target detection setup logic for followable and shootable targets
            _followableEnemyDetectorProvider = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                Transform, enemyFollowDetectionRangeInM, _targetFactories.RangeCalculatorProvider.BasicCalculator);

            ITargetFilter followTargetFilter = _targetFactories.FilterFactory.CreateTargetFilter(
                Helper.GetOppositeFaction(Faction), new List<TargetType> { TargetType.Aircraft, TargetType.Ships });
            _followableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(
                _followableEnemyDetectorProvider.TargetDetector, followTargetFilter);

            _followableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(
                _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_followableTargetFinder, _targetFactories.RankerFactory.EqualTargetRanker));
            _followableTargetProcessor.AddTargetConsumer(this);

            _shootableEnemyDetectorProvider = _cruiserSpecificFactories.Targets.DetectorFactory.CreateEnemyShipAndAircraftTargetDetector(
                Transform, Mathf.Max(_barrelControllers[0].TurretStats.RangeInM), _targetFactories.RangeCalculatorProvider.BasicCalculator);
            _shootableTargetFinder = _targetFactories.FinderFactory.CreateRangedTargetFinder(
                _shootableEnemyDetectorProvider.TargetDetector, _targetFactories.FilterFactory.CreateMulitpleExactMatchTargetFilter());

            _shootableTargetProcessor = _cruiserSpecificFactories.Targets.ProcessorFactory.CreateTargetProcessor(
                _cruiserSpecificFactories.Targets.TrackerFactory.CreateRankedTargetTracker(_shootableTargetFinder, _targetFactories.RankerFactory.EqualTargetRanker));

            foreach (var barrel in _barrelControllers)
            {
                _shootableTargetProcessor.AddTargetConsumer(barrel);
            }
        }
        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            return Helper.ConvertVectorsToPatrolPoints(_aircraftProvider.FindMissileFighterPatrolPoints(cruisingAltitudeInM));
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

            foreach (var barrel in _barrelControllers)
            {
                barrel.CleanUp();
            }
        }
    }
}
