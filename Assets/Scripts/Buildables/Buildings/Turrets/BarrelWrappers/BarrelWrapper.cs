using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class BarrelWrapper : MonoBehaviour, IBarrelWrapper
    {
        protected BarrelController[] _barrels;
        private TargetProcessorWrapper _targetProcessorWrapper;
        protected IFactoryProvider _factoryProvider;
        protected Faction _enemyFaction;
        protected float _minRangeInM;

        public Vector2 Position { get { return transform.position; } }
        public IDamageCapability DamageCapability { get; private set; }
        public float RangeInM { get; private set; }

        private List<Renderer> _renderers;
        public IList<Renderer> Renderers { get { return _renderers; } }

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

        private float _boostMultiplier;
        public float BoostMultiplier
        {
            get { return _boostMultiplier; }
            set
            {
                _boostMultiplier = value;

                foreach (IBarrelController barrel in _barrels)
                {
                    barrel.BoostMultiplier = _boostMultiplier;
                }
            }
        }

        public virtual void StaticInitialise()
        {
            _renderers = new List<Renderer>();

            InitialiseBarrels();

            DamageCapability = SumBarrelDamage();
            RangeInM = _barrels.Max(barrel => barrel.TurretStats.RangeInM);
            _minRangeInM = _barrels.Max(barrel => barrel.TurretStats.MinRangeInM);

            _targetProcessorWrapper = gameObject.GetComponentInChildren<TargetProcessorWrapper>();
            Assert.IsNotNull(_targetProcessorWrapper);
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
            ISoundKey firingSound = null)
        {
            Helper.AssertIsNotNull(parent, factoryProvider, enemyFaction);

            _factoryProvider = factoryProvider;
            _enemyFaction = enemyFaction;

            // Shared by all barrels
            ITargetFilter targetFilter = CreateTargetFilter();
            IAngleCalculator angleCalculator = CreateAngleCalculator();

            foreach (BarrelController barrel in _barrels)
            {
                IBarrelControllerArgs barrelArgs = CreateBarrelControllerArgs(barrel, parent, targetFilter, angleCalculator, firingSound);
                InitialiseBarrelController(barrel, barrelArgs);
            }

            ITargetProcessorArgs args
                = new TargetProcessorArgs(
                    _factoryProvider.TargetsFactory,
                    this,
                    _enemyFaction,
                    DamageCapability.AttackCapabilities,
                    RangeInM,
                    _minRangeInM);

            _targetProcessorWrapper.Initialise(args);
        }

        private IBarrelControllerArgs CreateBarrelControllerArgs(
            IBarrelController barrel,
            ITarget parent, 
            ITargetFilter targetFilter,
            IAngleCalculator angleCalculator,
            ISoundKey firingSound)
        {
            return new BarrelControllerArgs(
                targetFilter,
                CreateTargetPositionPredictor(),
                angleCalculator,
                CreateAccuracyAdjuster(angleCalculator, barrel),
                CreateRotationMovementController(barrel),
                CreatePositionValidator(),
                CreateAngleLimiter(),
                _factoryProvider,
                parent,
                firingSound);
        }

        protected virtual void InitialiseBarrelController(BarrelController barrel, IBarrelControllerArgs args)
        {
            barrel.Initialise(args);
        }

        public void StartAttackingTargets()
        {
            _targetProcessorWrapper.StartProvidingTargets();
        }

        protected virtual ITargetFilter CreateTargetFilter()
        {
            return _factoryProvider.TargetsFactory.CreateTargetFilter(_enemyFaction, DamageCapability.AttackCapabilities);
        }

        protected virtual ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateDummyPredictor();
        }

        protected abstract IAngleCalculator CreateAngleCalculator();

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
            return _factoryProvider.AccuracyAdjusterFactory.CreateDummyAdjuster();
        }

        protected virtual ITargetPositionValidator CreatePositionValidator()
        {
            // Default to all positions being valid
            return _factoryProvider.TargetPositionValidatorFactory.CreateDummyValidator();
        }

        protected virtual IAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.AngleLimiterFactory.CreateFacingLimiter();
        }

        public void Dispose()
        {
            _targetProcessorWrapper.Dispose();
            _targetProcessorWrapper = null;
        }
    }
}
