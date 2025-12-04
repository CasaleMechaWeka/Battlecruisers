using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets;
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
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Utils.PlatformAbstractions;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class PvPBarrelWrapper : MonoBehaviour, IPvPBarrelWrapper
    {
        protected PvPBarrelController[] _barrels;
        private PvPTargetProcessorWrapper _targetProcessorWrapper;
        private ITargetProcessor _targetProcessor;
        protected PvPCruiserSpecificFactories _cruiserSpecificFactories;
        protected Faction _enemyFaction;
        protected float _minRangeInM;
        public float minAngle = -30, maxAngle = 90;
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
            PvPCruiserSpecificFactories cruiserSpecificFactories,
            ObservableCollection<IBoostProvider> localBoostProviders = null,
            List<ObservableCollection<IBoostProvider>> globalFireRateBoostProviders = null,
            IAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(parent, cruiserSpecificFactories);

            _parent = parent;
            _enemyFaction = _parent.EnemyCruiser.Faction;
            _cruiserSpecificFactories = cruiserSpecificFactories;
            //Debug.Log(_enemyFaction);
            // Shared by all barrels
            ITargetFilter targetFilter = CreateTargetFilter();
            IAngleCalculator angleCalculator = CreateAngleCalculator(ProjectileStats);

            foreach (PvPBarrelController barrel in _barrels)
            {
                PvPBarrelControllerArgs barrelArgs
                    = CreateBarrelControllerArgs(
                        barrel,
                        parent,
                        targetFilter,
                        angleCalculator,
                        localBoostProviders ?? cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders,
                        globalFireRateBoostProviders ?? new List<ObservableCollection<IBoostProvider>>() { cruiserSpecificFactories.GlobalBoostProviders.DummyBoostProviders },
                        barrelFiringAnimation ?? GetBarrelAnimation());
                InitialiseBarrelController(barrel, barrelArgs);
            }

            IPvPTargetProcessorArgs args
                = new PvPTargetProcessorArgs(
                    _cruiserSpecificFactories,
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
            IAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(parent);

            _parent = parent;
            //    _enemyFaction = _parent.EnemyCruiser.Faction;
            foreach (PvPBarrelController barrel in _barrels)
            {
                PvPBarrelControllerArgs barrelArgs
                    = CreateBarrelControllerArgs(
                        parent,
                        barrelFiringAnimation ?? GetBarrelAnimation());
                InitialiseBarrelController_PvPClient(barrel, barrelArgs);
            }

        }


        // should be called by Server
        private PvPBarrelControllerArgs CreateBarrelControllerArgs(
            IBarrelController barrel,
            IPvPBuildable parent,
            ITargetFilter targetFilter,
            IAngleCalculator angleCalculator,
            ObservableCollection<IBoostProvider> localBoostProviders,
            List<ObservableCollection<IBoostProvider>> globalFireRateBoostProviders,
            IAnimation barrelFiringAnimation)
        {
            IUpdater updater = ChooseUpdater(PvPFactoryProvider.UpdaterProvider);

            return new PvPBarrelControllerArgs(
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
                globalFireRateBoostProviders,
                _parent.EnemyCruiser,
                barrelFiringAnimation);
        }

        // should be called by Client
        private PvPBarrelControllerArgs CreateBarrelControllerArgs(
            IPvPBuildable parent,
            IAnimation barrelFiringAnimation)
        {
            return new PvPBarrelControllerArgs(
                parent,
                barrelFiringAnimation);
        }

        // should be called by Server
        protected virtual void InitialiseBarrelController(PvPBarrelController barrel, PvPBarrelControllerArgs args)
        {
            _ = barrel.InitialiseAsync(args);
        }

        // should be called by Client
        protected virtual void InitialiseBarrelController_PvPClient(PvPBarrelController barrel, PvPBarrelControllerArgs args)
        {
            _ = barrel.InitialiseAsync_PvPClient(args);
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

        protected AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(minAngle, maxAngle);
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
