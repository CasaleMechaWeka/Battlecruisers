using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
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
        protected IList<TargetType> _attackCapabilities;
		protected float _minRangeInM;

        public Vector2 Position { get { return transform.position; } }
        public float DamagePerS { get; private set; }
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

                foreach (BarrelController barrel in _barrels)
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

				foreach (BarrelController barrel in _barrels)
                {
                    barrel.BoostMultiplier = _boostMultiplier;
                }
            }
        }

        public virtual void StaticInitialise()
        {
            _renderers = new List<Renderer>();

            InitialiseBarrels();

            DamagePerS = _barrels.Sum(barrel => barrel.DamagePerS);
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

        public void Initialise(IFactoryProvider factoryProvider, Faction enemyFaction, IList<TargetType> attackCapabilities)
        {
            Helper.AssertIsNotNull(factoryProvider, enemyFaction, attackCapabilities);

            _factoryProvider = factoryProvider;
            _enemyFaction = enemyFaction;
            _attackCapabilities = attackCapabilities;

            // Shared by all barrels
            ITargetFilter targetFilter = CreateTargetFilter();
            IAngleCalculator angleCalculator = CreateAngleCalculator();

			foreach (BarrelController barrel in _barrels)
            {
                InitialiseBarrelController(barrel, targetFilter, angleCalculator);
            }

            _targetProcessorWrapper
                .Initialise(
                    _factoryProvider.TargetsFactory,
                    this,
                    _enemyFaction,
                    _attackCapabilities,
                    RangeInM,
                    _minRangeInM);
        }

        protected virtual void InitialiseBarrelController(BarrelController barrel, ITargetFilter targetFilter, IAngleCalculator angleCalculator)
        {
            IBarrelControllerArgs args
                = new BarrelControllerArgs(
                    targetFilter,
                    CreateTargetPositionPredictor(),
                    angleCalculator,
                    CreateAccuracyAdjuster(angleCalculator, barrel),
                    CreateRotationMovementController(barrel),
                    CreatePositionValidator(),
                    _factoryProvider);

            barrel.Initialise(args);
        }

        public void StartAttackingTargets()
        {
            _targetProcessorWrapper.StartProvidingTargets();
        }

        protected ITargetFilter CreateTargetFilter()
        {
            return _factoryProvider.TargetsFactory.CreateTargetFilter(_enemyFaction, _attackCapabilities);
        }

        protected virtual ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateDummyPredictor();
        }

        protected abstract IAngleCalculator CreateAngleCalculator();

        protected virtual IRotationMovementController CreateRotationMovementController(BarrelController barrel)
        {
            return 
                _factoryProvider.MovementControllerFactory.CreateRotationMovementController(
                    barrel.TurretStats.TurretRotateSpeedInDegrees, 
                    barrel.transform);
        }

        protected virtual IAccuracyAdjuster CreateAccuracyAdjuster(IAngleCalculator angleCalculator, BarrelController barrel)
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
            // Default to allowing all angles
            return _factoryProvider.AngleLimiterFactory.CreateDummyLimiter();
        }

        public void Dispose()
        {
            _targetProcessorWrapper.Dispose();
            _targetProcessorWrapper = null;
        }
    }
}
