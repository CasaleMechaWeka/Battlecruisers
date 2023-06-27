using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public abstract class PvPBarrelWrapper : MonoBehaviour, IPvPBarrelWrapper
    {
        protected PvPBarrelController[] _barrels;
        private PvPTargetProcessorWrapper _targetProcessorWrapper;
        private IPvPTargetProcessor _targetProcessor;
        protected IPvPFactoryProvider _factoryProvider;
        protected IPvPCruiserSpecificFactories _cruiserSpecificFactories;
        protected PvPFaction _enemyFaction;
        protected float _minRangeInM;
        private IPvPBuildable _parent;

        public Vector2 Position => transform.position;
        public IPvPDamageCapability DamageCapability { get; private set; }
        public float RangeInM { get; private set; }

        private List<SpriteRenderer> _renderers;
        public IList<SpriteRenderer> Renderers => _renderers;

        private IPvPTarget _target;
        public IPvPTarget Target
        {
            get { return _target; }
            set
            {
                // When Unity game object is destroyed need to null check it, even though it is not truly null.
                // Logging.Log(Tags.BARREL_WRAPPER, $"Parent: {_parent}  _target: {PrintTarget(_target)} > {PrintTarget(value)}");
                _target = value;

                foreach (IPvPBarrelController barrel in _barrels)
                {
                    barrel.Target = _target;
                }
            }
        }

        // Assumes all barrel projectile stats are the same.
        private IPvPProjectileStats ProjectileStats
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
            RangeInM = _barrels.Max(barrel => barrel.pvpTurretStats.RangeInM);
            _minRangeInM = _barrels.Max(barrel => barrel.pvpTurretStats.MinRangeInM);
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

        private IPvPDamageCapability SumBarrelDamage()
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


        // should be called by server
        public void Initialise(
            IPvPBuildable parent,
            IPvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            IPvPSoundKey firingSound = null,
            ObservableCollection<IPvPBoostProvider> localBoostProviders = null,
            ObservableCollection<IPvPBoostProvider> globalFireRateBoostProviders = null,
            IPvPAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(parent, factoryProvider, cruiserSpecificFactories);

            _parent = parent;
            _enemyFaction = _parent.EnemyCruiser.Faction;
            _factoryProvider = factoryProvider;
            _cruiserSpecificFactories = cruiserSpecificFactories;
            //Debug.Log(_enemyFaction);
            // Shared by all barrels
            IPvPTargetFilter targetFilter = CreateTargetFilter();
            IPvPAngleCalculator angleCalculator = CreateAngleCalculator(ProjectileStats);
            IPvPAttackablePositionFinder attackablePositionFinder = CreateAttackablePositionFinder();

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
            IPvPSoundKey firingSound = null,
            IPvPAnimation barrelFiringAnimation = null)
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
            IPvPBarrelController barrel,
            IPvPBuildable parent,
            IPvPTargetFilter targetFilter,
            IPvPAngleCalculator angleCalculator,
            IPvPAttackablePositionFinder attackablePositionFinder,
            IPvPSoundKey firingSound,
            ObservableCollection<IPvPBoostProvider> localBoostProviders,
            ObservableCollection<IPvPBoostProvider> globalFireRateBoostProvider,
            IPvPAnimation barrelFiringAnimation)
        {
            IPvPUpdater updater = ChooseUpdater(_factoryProvider.UpdaterProvider);

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
            IPvPBarrelController barrel,
            IPvPBuildable parent,
            IPvPSoundKey firingSound,
            IPvPAnimation barrelFiringAnimation)
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
            barrel.InitialiseAsync(args);
        }

        // should be called by Client
        protected virtual void InitialiseBarrelController_PvPClient(PvPBarrelController barrel, IPvPBarrelControllerArgs args)
        {
            barrel.InitialiseAsync_PvPClient(args);
        }

        protected virtual IPvPTargetFilter CreateTargetFilter()
        {
            return _factoryProvider.Targets.FilterFactory.CreateTargetFilter(_enemyFaction, DamageCapability.AttackCapabilities);
        }

        protected virtual IPvPTargetPositionPredictor CreateTargetPositionPredictor()
        {
            return _factoryProvider.TargetPositionPredictorFactory.CreateDummyPredictor();
        }

        protected abstract IPvPAngleCalculator CreateAngleCalculator(IPvPProjectileStats projectileStats);

        private IPvPAttackablePositionFinder CreateAttackablePositionFinder()
        {
            IPvPAttackablePositionFinderWrapper positionFinderWrapper = GetComponent<IPvPAttackablePositionFinderWrapper>();

            if (positionFinderWrapper != null)
            {
                return positionFinderWrapper.CreatePositionFinder();
            }
            else
            {
                return _factoryProvider.Turrets.AttackablePositionFinderFactory.DummyPositionFinder;
            }
        }

        protected virtual IPvPRotationMovementController CreateRotationMovementController(IPvPBarrelController barrel, IPvPDeltaTimeProvider deltaTimeProvider)
        {
            return
                _factoryProvider.MovementControllerFactory.CreateRotationMovementController(
                    barrel.pvpTurretStats.TurretRotateSpeedInDegrees,
                    barrel.Transform,
                    deltaTimeProvider);
        }

        protected virtual IPvPAccuracyAdjuster CreateAccuracyAdjuster(IPvPAngleCalculator angleCalculator, IPvPBarrelController barrel)
        {
            // Default to 100% accuracy
            return _factoryProvider.Turrets.AccuracyAdjusterFactory.CreateDummyAdjuster();
        }

        protected virtual IPvPTargetPositionValidator CreatePositionValidator()
        {
            // Default to all positions being valid
            return _factoryProvider.Turrets.TargetPositionValidatorFactory.CreateDummyValidator();
        }

        protected virtual IPvPAngleLimiter CreateAngleLimiter()
        {
            return _factoryProvider.Turrets.AngleLimiterFactory.CreateFacingLimiter();
        }

        protected virtual IPvPUpdater ChooseUpdater(IPvPUpdaterProvider updaterProvider)
        {
            return updaterProvider.BarrelControllerUpdater;
        }

        private IPvPAnimation GetBarrelAnimation()
        {
            IPvPAnimationInitialiser animationInitialiser = GetComponent<IPvPAnimationInitialiser>();
            return animationInitialiser?.CreateAnimation();
        }

        private string PrintTarget(IPvPTarget target)
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
