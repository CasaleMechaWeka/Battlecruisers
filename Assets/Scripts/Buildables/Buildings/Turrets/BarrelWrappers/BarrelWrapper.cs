using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Effects;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class BarrelWrapper : MonoBehaviour, IBarrelWrapper
    {
        protected BarrelController[] _barrels;
        private ITargetProcessor _targetProcessor;
        protected IFactoryProvider _factoryProvider;
        protected ICruiserSpecificFactories _cruiserSpecificFactories;
        protected Faction _enemyFaction;
        protected float _minRangeInM;

        public Vector2 Position => transform.position;
        public IDamageCapability DamageCapability { get; private set; }
        public float RangeInM { get; private set; }

        private List<SpriteRenderer> _renderers;
        public IList<SpriteRenderer> Renderers => _renderers;

        private ITarget _target;
        public ITarget Target
        {
            get { return _target; }
            set
            {
                // When Unity game object is destroyed need to null check it, even though it is not truly null.
                Logging.Log(Tags.BARREL_WRAPPER, $"_target: {_target} > {value?.ToString()}");
                _target = value;

                foreach (IBarrelController barrel in _barrels)
                {
                    barrel.Target = _target;
                }
            }
        }

        // Assumes all barrel projectile stats are the same.
        private IProjectileStats ProjectileStats
        {
            get
            {
                Assert.IsTrue(_barrels.Length != 0);
                return _barrels[0].ProjectileStats;
            }
        }

        public virtual void StaticInitialise()
        {
            _renderers = new List<SpriteRenderer>();

            InitialiseBarrels();

            DamageCapability = SumBarrelDamage();
            RangeInM = _barrels.Max(barrel => barrel.TurretStats.RangeInM);
            _minRangeInM = _barrels.Max(barrel => barrel.TurretStats.MinRangeInM);
        }

        private void InitialiseBarrels()
        {
            _barrels = gameObject.GetComponentsInChildren<BarrelController>();
            Assert.IsTrue(_barrels.Length != 0);

            foreach (BarrelController barrel in _barrels)
            {
                barrel.StaticInitialise();
                _renderers.AddRange(barrel.Renderers);
            }
        }

        private IDamageCapability SumBarrelDamage()
        {
            if (_barrels.Length == 1)
            {
                return _barrels[0].DamageCapability;
            }
            else
            {
                float totalDamagePerS = _barrels.Sum(barrel => barrel.DamageCapability.DamagePerS);
                return new DamageCapability(totalDamagePerS, _barrels[0].DamageCapability.AttackCapabilities);
            }
        }

        public void Initialise(
            ITarget parent, 
            IFactoryProvider factoryProvider,
            ICruiserSpecificFactories cruiserSpecificFactories,
            Faction enemyFaction,
            ISoundKey firingSound = null,
            ObservableCollection<IBoostProvider> localBoostProviders = null,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders = null,
            IAnimation barrelFiringAnimation = null)
        {
            Helper.AssertIsNotNull(parent, factoryProvider, cruiserSpecificFactories);

            _factoryProvider = factoryProvider;
            _cruiserSpecificFactories = cruiserSpecificFactories;
            _enemyFaction = enemyFaction;

            // Shared by all barrels
            ITargetFilter targetFilter = CreateTargetFilter();
            IAngleCalculator angleCalculator = CreateAngleCalculator(ProjectileStats);
            IAttackablePositionFinder attackablePositionFinder = CreateAttackablePositionFinder();

            foreach (BarrelController barrel in _barrels)
            {
                IBarrelControllerArgs barrelArgs 
                    = CreateBarrelControllerArgs(
                        barrel, 
                        parent, 
                        targetFilter, 
                        angleCalculator, 
                        attackablePositionFinder, 
                        firingSound, 
                        localBoostProviders ?? cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        globalFireRateBoostProviders ?? cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        barrelFiringAnimation ?? GetBarrelAnimation());
                InitialiseBarrelController(barrel, barrelArgs);
            }

            ITargetProcessorArgs args
                = new TargetProcessorArgs(
                    _cruiserSpecificFactories,
                    _factoryProvider.Targets,
                    _enemyFaction,
                    DamageCapability.AttackCapabilities,
                    RangeInM,
                    _minRangeInM);

            TargetProcessorWrapper targetProcessorWrapper = gameObject.GetComponentInChildren<TargetProcessorWrapper>();
            Assert.IsNotNull(targetProcessorWrapper);
            _targetProcessor = targetProcessorWrapper.CreateTargetProcessor(args);
            _targetProcessor.AddTargetConsumer(this);
        }

        private IBarrelControllerArgs CreateBarrelControllerArgs(
            IBarrelController barrel,
            ITarget parent, 
            ITargetFilter targetFilter,
            IAngleCalculator angleCalculator,
            IAttackablePositionFinder attackablePositionFinder,
            ISoundKey firingSound,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProvider,
            IAnimation barrelFiringAnimation)
        {
            IUpdater updater = ChooseUpdater(_factoryProvider.UpdaterProvider);

            return new BarrelControllerArgs(
                updater,
                targetFilter,
                CreateTargetPositionPredictor(),
                angleCalculator,
                attackablePositionFinder,
                CreateAccuracyAdjuster(angleCalculator, barrel),
                CreateRotationMovementController(barrel, updater),
                CreatePositionValidator(),
                CreateAngleLimiter(),
                _factoryProvider,
                _cruiserSpecificFactories,
                parent,
                localBoostProviders,
                globalFireRateBoostProvider,
                firingSound,
                barrelFiringAnimation);
        }

        protected virtual void InitialiseBarrelController(BarrelController barrel, IBarrelControllerArgs args)
        {
            barrel.InitialiseAsync(args);
        }

        protected virtual ITargetFilter CreateTargetFilter()
        {
            return _factoryProvider.Targets.FilterFactory.CreateTargetFilter(_enemyFaction, DamageCapability.AttackCapabilities);
        }

        protected virtual ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateDummyPredictor();
        }

        protected abstract IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats);

        private IAttackablePositionFinder CreateAttackablePositionFinder()
        {
            IAttackablePositionFinderWrapper positionFinderWrapper = GetComponent<IAttackablePositionFinderWrapper>();

            if (positionFinderWrapper != null)
            {
                return positionFinderWrapper.CreatePositionFinder();
            }
            else
            {
                return _factoryProvider.Turrets.AttackablePositionFinderFactory.DummyPositionFinder;
            }
        }

        protected virtual IRotationMovementController CreateRotationMovementController(IBarrelController barrel, IDeltaTimeProvider deltaTimeProvider)
        {
            return 
                _factoryProvider.MovementControllerFactory.CreateRotationMovementController(
                    barrel.TurretStats.TurretRotateSpeedInDegrees, 
                    barrel.Transform,
                    deltaTimeProvider);
        }

        protected virtual IAccuracyAdjuster CreateAccuracyAdjuster(IAngleCalculator angleCalculator, IBarrelController barrel)
        {
            // Default to 100% accuracy
            return _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateDummyAdjuster();
        }

        protected virtual ITargetPositionValidator CreatePositionValidator()
        {
            // Default to all positions being valid
            return _factoryProvider.Turrets.TargetPositionValidatorFactory.CreateDummyValidator();
        }

        protected virtual IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateFacingLimiter();
        }

        protected virtual IUpdater ChooseUpdater(IUpdaterProvider updaterProvider)
        {
            return updaterProvider.BarrelControllerUpdater;
        }

        private IAnimation GetBarrelAnimation()
        {
            IAnimationInitialiser animationInitialiser = GetComponent<IAnimationInitialiser>();
            return animationInitialiser != null ? animationInitialiser.CreateAnimation() : null;
        }

        public void DisposeManagedState()
        {
            _targetProcessor?.DisposeManagedState();

            foreach (BarrelController barrel in _barrels)
            {
                barrel.CleanUp();
            }
        }
    }
}
