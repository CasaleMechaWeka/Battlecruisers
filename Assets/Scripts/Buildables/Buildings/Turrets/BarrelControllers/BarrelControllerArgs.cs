using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class BarrelControllerArgs : IBarrelControllerArgs
    {
        public IUpdater Updater { get; }
        public ITargetFilter TargetFilter { get; }
        public ITargetPositionPredictor TargetPositionPredictor { get; }
        public IAngleCalculator AngleCalculator { get; }
        public IAttackablePositionFinder AttackablePositionFinder { get; }
        public IAccuracyAdjuster AccuracyAdjuster { get; }
        public IRotationMovementController RotationMovementController { get; }
        public IFactoryProvider FactoryProvider { get; }
        public ITargetPositionValidator TargetPositionValidator { get; }
        public IAngleLimiter AngleLimiter { get; }
        public ITarget Parent { get; }
        public ObservableCollection<IBoostProvider> LocalBoostProviders { get; }
        public ObservableCollection<IBoostProvider> GlobalFireRateBoostProviders { get; }
        public ISoundKey SpawnerSoundKey { get; }

        public BarrelControllerArgs(
            IUpdater updater,
            ITargetFilter targetFilter,
            ITargetPositionPredictor targetPositionPredictor,
            IAngleCalculator angleCalculator,
            IAttackablePositionFinder attackablePositionFinder,
            IAccuracyAdjuster accuracyAdjuster,
            IRotationMovementController rotationMovementController,
            ITargetPositionValidator targetPositionValidator,
            IAngleLimiter angleLimiter,
            IFactoryProvider factoryProvider,
            ITarget parent,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProvider,
            ISoundKey firingSound = null)
        {
            Helper.AssertIsNotNull(
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
                parent,
                localBoostProviders,
                globalFireRateBoostProvider);

            Updater = updater;
            TargetFilter = targetFilter;
            TargetPositionPredictor = targetPositionPredictor;
            AngleCalculator = angleCalculator;
            AttackablePositionFinder = attackablePositionFinder;
            AccuracyAdjuster = accuracyAdjuster;
            RotationMovementController = rotationMovementController;
            FactoryProvider = factoryProvider;
            AngleLimiter = angleLimiter;
            TargetPositionValidator = targetPositionValidator;
            Parent = parent;
            LocalBoostProviders = localBoostProviders;
            GlobalFireRateBoostProviders = globalFireRateBoostProvider;
            SpawnerSoundKey = firingSound;
        }
    }
}
