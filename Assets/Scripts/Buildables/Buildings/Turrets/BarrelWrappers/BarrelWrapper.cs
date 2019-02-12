using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class BarrelWrapper : MonoBehaviour, IBarrelWrapper
    {
        protected BarrelController[] _barrels;
        private ITargetProcessor _targetProcessor;
        protected IFactoryProvider _factoryProvider;
        protected Faction _enemyFaction;
        protected float _minRangeInM;

        public Vector2 Position { get { return transform.position; } }
        public IDamageCapability DamageCapability { get; private set; }
        public float RangeInM { get; private set; }

        private List<SpriteRenderer> _renderers;
        public IList<SpriteRenderer> Renderers { get { return _renderers; } }

        private ITarget _target;
        public ITarget Target
        {
            get { return _target; }
            set
            {
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
            Faction enemyFaction,
            ISoundKey firingSound = null,
            IObservableCollection<IBoostProvider> localBoostProviders = null,
            IObservableCollection<IBoostProvider> globalFireRateBoostProviders = null)
        {
            Helper.AssertIsNotNull(parent, factoryProvider);

            _factoryProvider = factoryProvider;
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
                        localBoostProviders ?? factoryProvider.GlobalBoostProviders.DummyBoostProviders,
                        globalFireRateBoostProviders ?? factoryProvider.GlobalBoostProviders.DummyBoostProviders);
                InitialiseBarrelController(barrel, barrelArgs);
            }

            ITargetProcessorArgs args
                = new TargetProcessorArgs(
                    _factoryProvider.TargetFactories,
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
            IObservableCollection<IBoostProvider> localBoostProviders,
            IObservableCollection<IBoostProvider> globalFireRateBoostProvider)
        {
            return new BarrelControllerArgs(
                targetFilter,
                CreateTargetPositionPredictor(),
                angleCalculator,
                attackablePositionFinder,
                CreateAccuracyAdjuster(angleCalculator, barrel),
                CreateRotationMovementController(barrel),
                CreatePositionValidator(),
                CreateAngleLimiter(),
                _factoryProvider,
                parent,
                localBoostProviders,
                globalFireRateBoostProvider,
                firingSound);
        }

        protected virtual void InitialiseBarrelController(BarrelController barrel, IBarrelControllerArgs args)
        {
            barrel.Initialise(args);
        }

        protected virtual ITargetFilter CreateTargetFilter()
        {
            return _factoryProvider.TargetFactories.FilterFactory.CreateTargetFilter(_enemyFaction, DamageCapability.AttackCapabilities);
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

        protected virtual IRotationMovementController CreateRotationMovementController(IBarrelController barrel)
        {
            return 
                _factoryProvider.MovementControllerFactory.CreateRotationMovementController(
                    barrel.TurretStats.TurretRotateSpeedInDegrees, 
                    barrel.Transform);
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

        public void DisposeManagedState()
        {
            _targetProcessor.DisposeManagedState();

            foreach (BarrelController barrel in _barrels)
            {
                barrel.CleanUp();
            }
        }
    }
}
