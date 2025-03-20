using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Effects;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPBarrelControllerArgs : IPvPBarrelControllerArgs
    {
        public IUpdater Updater { get; }
        public ITargetFilter TargetFilter { get; }
        public ITargetPositionPredictor TargetPositionPredictor { get; }
        public IAngleCalculator AngleCalculator { get; }
        public IAttackablePositionFinder AttackablePositionFinder { get; }
        public AccuracyAdjuster AccuracyAdjuster { get; }
        public IRotationMovementController RotationMovementController { get; }
        public IPvPFactoryProvider FactoryProvider { get; }
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public FacingMinRangePositionValidator TargetPositionValidator { get; }
        public AngleLimiter AngleLimiter { get; }
        public ITarget Parent { get; }
        public ObservableCollection<IBoostProvider> LocalBoostProviders { get; }
        public ObservableCollection<IBoostProvider> GlobalFireRateBoostProviders { get; }
        public ISoundKey SpawnerSoundKey { get; }
        public IAnimation BarrelFiringAnimation { get; }
        public IPvPCruiser EnemyCruiser { get; }

        // should be called by server
        public PvPBarrelControllerArgs(
            IUpdater updater,
            ITargetFilter targetFilter,
            ITargetPositionPredictor targetPositionPredictor,
            IAngleCalculator angleCalculator,
            IAttackablePositionFinder attackablePositionFinder,
            AccuracyAdjuster accuracyAdjuster,
            IRotationMovementController rotationMovementController,
            FacingMinRangePositionValidator targetPositionValidator,
            AngleLimiter angleLimiter,
            IPvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            ITarget parent,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProvider,
            IPvPCruiser enemyCruiser,
            ISoundKey firingSound = null,
            IAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(
                updater,
                targetFilter,
                targetPositionPredictor,
                angleCalculator,
                attackablePositionFinder,
                accuracyAdjuster,
                rotationMovementController,
                targetPositionValidator,
                angleLimiter,
                factoryProvider,
                cruiserSpecificFactories,
                parent,
                localBoostProviders,
                globalFireRateBoostProvider,
                enemyCruiser);

            Updater = updater;
            TargetFilter = targetFilter;
            TargetPositionPredictor = targetPositionPredictor;
            AngleCalculator = angleCalculator;
            AttackablePositionFinder = attackablePositionFinder;
            AccuracyAdjuster = accuracyAdjuster;
            RotationMovementController = rotationMovementController;
            FactoryProvider = factoryProvider;
            CruiserSpecificFactories = cruiserSpecificFactories;
            AngleLimiter = angleLimiter;
            TargetPositionValidator = targetPositionValidator;
            Parent = parent;
            LocalBoostProviders = localBoostProviders;
            GlobalFireRateBoostProviders = globalFireRateBoostProvider;
            EnemyCruiser = enemyCruiser;
            SpawnerSoundKey = firingSound;
            BarrelFiringAnimation = barrelFiringAnimation ?? new DummyAnimation();
        }


        // should be called by client

        public PvPBarrelControllerArgs(
            IPvPFactoryProvider factoryProvider,
            ITarget parent,
            ISoundKey firingSound = null,
            IAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(parent);
            Parent = parent;
            SpawnerSoundKey = firingSound;
            BarrelFiringAnimation = barrelFiringAnimation ?? new DummyAnimation();
        }
    }
}
