using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Effects;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class PvPBarrelWrapper : MonoBehaviour, IPvPBarrelWrapper
    {
        protected PvPBarrelController[] _barrels;
        private PvPTargetProcessorWrapper _targetProcessorWrapper;
        private ITargetProcessor _targetProcessor;
        protected IPvPFactoryProvider _factoryProvider;
        protected IPvPCruiserSpecificFactories _cruiserSpecificFactories;
        protected Faction _enemyFaction;
        protected float _minRangeInM;
        private IPvPBuildable _parent;

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
                Debug.Log("[PvPBarrelWrapper] Target set to: " + value);
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
            _barrels = gameObject.GetComponentsInChildren<PvPBarrelController>();
            Assert.IsTrue(_barrels.Length != 0);

            foreach (PvPBarrelController barrel in _barrels)
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
                return new PvPDamageCapability(totalDamagePerS, _barrels[0].DamageCapability.AttackCapabilities);
            }
        }

        public void ApplyVariantStats(IPvPBuilding building)
        {
            foreach (PvPBarrelController barrel in _barrels)
            {
                barrel.ApplyVariantStats(building);
            }
        }

        public void ApplyVariantStats(IPvPUnit unit)
        {
            foreach (PvPBarrelController barrel in _barrels)
            {
                barrel.ApplyVariantStats(unit);
            }
        }

        // should be called by server
        public void Initialise(
            IPvPBuildable parent,
            IPvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            ISoundKey firingSound = null,
            ObservableCollection<IBoostProvider> localBoostProviders = null,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders = null,
            IAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(parent, factoryProvider, cruiserSpecificFactories);

            _parent = parent;
            _enemyFaction = _parent.EnemyCruiser.Faction;
            _factoryProvider = factoryProvider;
            _cruiserSpecificFactories = cruiserSpecificFactories;
            //Debug.Log(_enemyFaction);
            // Shared by all barrels
            ITargetFilter targetFilter = CreateTargetFilter();
            IAngleCalculator angleCalculator = CreateAngleCalculator(ProjectileStats);
            ClosestPositionFinder attackablePositionFinder = new ClosestPositionFinder();

            foreach (PvPBarrelController barrel in _barrels)
            {
                IPvPBarrelControllerArgs barrelArgs
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

            IPvPTargetProcessorArgs args
                = new PvPTargetProcessorArgs(
                    _cruiserSpecificFactories,
                    _factoryProvider.Targets,
                    _enemyFaction,
                    DamageCapability.AttackCapabilities,
                    RangeInM,
                    _minRangeInM,
                    parent);

            _targetProcessorWrapper = gameObject.GetComponentInChildren<PvPTargetProcessorWrapper>();
            Assert.IsNotNull(_targetProcessorWrapper);
            _targetProcessor = _targetProcessorWrapper.CreateTargetProcessor(args);
            _targetProcessor.AddTargetConsumer(this);

        }

        // should be called by client
        public void Initialise(
            IPvPBuildable parent,
            IPvPFactoryProvider factoryProvider,
            ISoundKey firingSound = null,
            IAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(parent, factoryProvider);

            _parent = parent;
            //    _enemyFaction = _parent.EnemyCruiser.Faction;
            _factoryProvider = factoryProvider;
            foreach (PvPBarrelController barrel in _barrels)
            {
                IPvPBarrelControllerArgs barrelArgs
                    = CreateBarrelControllerArgs(
                        barrel,
                        parent,
                        firingSound,
                        barrelFiringAnimation ?? GetBarrelAnimation());
                InitialiseBarrelController_PvPClient(barrel, barrelArgs);
            }

        }


        // should be called by Server
        private IPvPBarrelControllerArgs CreateBarrelControllerArgs(
            IBarrelController barrel,
            IPvPBuildable parent,
            ITargetFilter targetFilter,
            IAngleCalculator angleCalculator,
            ClosestPositionFinder attackablePositionFinder,
            ISoundKey firingSound,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProvider,
            IAnimation barrelFiringAnimation)
        {
            IUpdater updater = ChooseUpdater(_factoryProvider.UpdaterProvider);

            return new PvPBarrelControllerArgs(
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
                _parent.EnemyCruiser,
                firingSound,
                barrelFiringAnimation);
        }

        // should be called by Client
        private IPvPBarrelControllerArgs CreateBarrelControllerArgs(
            IBarrelController barrel,
            IPvPBuildable parent,
            ISoundKey firingSound,
            IAnimation barrelFiringAnimation)
        {
            return new PvPBarrelControllerArgs(
                _factoryProvider,
                parent,
                firingSound,
                barrelFiringAnimation);
        }

        // should be called by Server
        protected virtual void InitialiseBarrelController(PvPBarrelController barrel, IPvPBarrelControllerArgs args)
        {
            _ = barrel.InitialiseAsync(args);
        }

        // should be called by Client
        protected virtual void InitialiseBarrelController_PvPClient(PvPBarrelController barrel, IPvPBarrelControllerArgs args)
        {
            _ = barrel.InitialiseAsync_PvPClient(args);
        }

        protected virtual ITargetFilter CreateTargetFilter()
        {
            return _factoryProvider.Targets.FilterFactory.CreateTargetFilter(_enemyFaction, DamageCapability.AttackCapabilities);
        }

        protected virtual ITargetPositionPredictor CreateTargetPositionPredictor()
        {
            return new DummyTargetPositionPredictor();
        }

        protected abstract IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats);

        protected virtual IRotationMovementController CreateRotationMovementController(IBarrelController barrel, IDeltaTimeProvider deltaTimeProvider)
        {
            return
                _factoryProvider.MovementControllerFactory.CreateRotationMovementController(
                    barrel.TurretStats.TurretRotateSpeedInDegrees,
                    barrel.Transform,
                    deltaTimeProvider);
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

            foreach (PvPBarrelController barrel in _barrels)
            {
                barrel.CleanUp();
            }
        }
    }
}
