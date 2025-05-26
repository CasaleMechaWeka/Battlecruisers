using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
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
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class BarrelWrapper : MonoBehaviour, IBarrelWrapper
    {
        protected BarrelController[] _barrels;
        private TargetProcessorWrapper _targetProcessorWrapper;
        private ITargetProcessor _targetProcessor;
        protected CruiserSpecificFactories _cruiserSpecificFactories;
        protected Faction _enemyFaction;
        protected float _minRangeInM;
        private IBuildable _parent;

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
                Logging.Log(Tags.BARREL_WRAPPER, $"Parent: {_parent}  _target: {PrintTarget(_target)} > {PrintTarget(value)}");
                _target = value;

                foreach (IBarrelController barrel in _barrels)
                {
                    barrel.Target = _target;
                }
            }
        }

        // Assumes all barrel projectile stats are the same.
        private ProjectileStats ProjectileStats
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
        public void ApplyVariantStats(IBuilding building)
        {
            foreach (BarrelController barrel in _barrels)
            {
                barrel.ApplyVariantStats(building);
            }
        }

        public void ApplyVariantStats(IUnit unit)
        {
            foreach (BarrelController barrel in _barrels)
            {
                barrel.ApplyVariantStats(unit);
            }
        }

        public void Initialise(
            IBuildable parent,
            CruiserSpecificFactories cruiserSpecificFactories,
            SoundKey firingSound = null,
            ObservableCollection<IBoostProvider> localBoostProviders = null,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders = null,
            IAnimation barrelFiringAnimation = null)
        {
            Helper.AssertIsNotNull(parent, cruiserSpecificFactories);

            _parent = parent;
            _enemyFaction = _parent.EnemyCruiser.Faction;
            _cruiserSpecificFactories = cruiserSpecificFactories;
            //Debug.Log(_enemyFaction);
            // Shared by all barrels
            ITargetFilter targetFilter = CreateTargetFilter();
            IAngleCalculator angleCalculator = CreateAngleCalculator(ProjectileStats);

            foreach (BarrelController barrel in _barrels)
            {
                BarrelControllerArgs barrelArgs
                    = CreateBarrelControllerArgs(
                        barrel,
                        parent,
                        targetFilter,
                        angleCalculator,
                        firingSound,
                        localBoostProviders ?? cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        globalFireRateBoostProviders ?? cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        barrelFiringAnimation ?? GetBarrelAnimation());
                InitialiseBarrelController(barrel, barrelArgs);
            }

            TargetProcessorArgs args
                = new TargetProcessorArgs(
                    _cruiserSpecificFactories,
                    FactoryProvider.Targets,
                    _enemyFaction,
                    DamageCapability.AttackCapabilities,
                    RangeInM,
                    _minRangeInM,
                    parent);

            _targetProcessorWrapper = gameObject.GetComponentInChildren<TargetProcessorWrapper>();
            Assert.IsNotNull(_targetProcessorWrapper);
            _targetProcessor = _targetProcessorWrapper.CreateTargetProcessor(args);
            _targetProcessor.AddTargetConsumer(this);
        }

        private BarrelControllerArgs CreateBarrelControllerArgs(
            IBarrelController barrel,
            IBuildable parent,
            ITargetFilter targetFilter,
            IAngleCalculator angleCalculator,
            SoundKey firingSound,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProvider,
            IAnimation barrelFiringAnimation)
        {
            IUpdater updater = ChooseUpdater(FactoryProvider.UpdaterProvider);

            return new BarrelControllerArgs(
                updater,
                targetFilter,
                CreateTargetPositionPredictor(),
                angleCalculator,
                CreateAccuracyAdjuster(angleCalculator, barrel),
                CreateRotationMovementController(barrel, updater),
                CreatePositionValidator(),
                CreateAngleLimiter(),
                _cruiserSpecificFactories,
                parent,
                localBoostProviders,
                globalFireRateBoostProvider,
                _parent.EnemyCruiser,
                firingSound,
                barrelFiringAnimation);
        }

        protected virtual void InitialiseBarrelController(BarrelController barrel, BarrelControllerArgs args)
        {
            _ = barrel.InitialiseAsync(args);
        }

        protected virtual ITargetFilter CreateTargetFilter()
        {
            return new FactionAndTargetTypeFilter(_enemyFaction, DamageCapability.AttackCapabilities);
        }

        protected virtual ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return new DummyTargetPositionPredictor();
        }

        protected abstract IAngleCalculator CreateAngleCalculator(ProjectileStats projectileStats);

        protected virtual IRotationMovementController CreateRotationMovementController(IBarrelController barrel, IDeltaTimeProvider deltaTimeProvider)
        {
            return
                new RotationMovementController(
                    new TransformBC(barrel.Transform),
                    deltaTimeProvider,
                    barrel.TurretStats.TurretRotateSpeedInDegrees);
        }

        protected virtual AccuracyAdjuster CreateAccuracyAdjuster(IAngleCalculator angleCalculator, IBarrelController barrel)
        {
            // Default to 100% accuracy
            return new AccuracyAdjuster((0, 0));
        }

        protected virtual FacingMinRangePositionValidator CreatePositionValidator()
        {
            // Default to all positions being valid
            return new FacingMinRangePositionValidator(0, true);
        }

        protected virtual AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-30, 90);
        }

        protected virtual IUpdater ChooseUpdater(IUpdaterProvider updaterProvider)
        {
            return updaterProvider.BarrelControllerUpdater;
        }

        private IAnimation GetBarrelAnimation()
        {
            IAnimationInitialiser animationInitialiser = GetComponent<IAnimationInitialiser>();
            return animationInitialiser?.CreateAnimation();
        }

        private string PrintTarget(ITarget target)
        {
            return target?.ToString() ?? "null";
        }

        public void DisposeManagedState()
        {
            Target = null;

            _targetProcessor?.RemoveTargetConsumer(this);
            _targetProcessorWrapper?.DisposeManagedState();

            foreach (BarrelController barrel in _barrels)
            {
                barrel.CleanUp();
            }
        }
    }
}
