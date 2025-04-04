using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Cruisers;
using BattleCruisers.Effects;
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
    public class BarrelControllerArgs
    {
        public IUpdater Updater { get; }
        public ITargetFilter TargetFilter { get; }
        public ITargetPositionPredictor TargetPositionPredictor { get; }
        public IAngleCalculator AngleCalculator { get; }
        public AccuracyAdjuster AccuracyAdjuster { get; }
        public IRotationMovementController RotationMovementController { get; }
        public CruiserSpecificFactories CruiserSpecificFactories { get; }
        public FacingMinRangePositionValidator TargetPositionValidator { get; }
        public AngleLimiter AngleLimiter { get; }
        public ITarget Parent { get; }
        public ObservableCollection<IBoostProvider> LocalBoostProviders { get; }
        public ObservableCollection<IBoostProvider> GlobalFireRateBoostProviders { get; }
        public ISoundKey SpawnerSoundKey { get; }
        public IAnimation BarrelFiringAnimation { get; }
        public ICruiser EnemyCruiser { get; }

        public BarrelControllerArgs(
            IUpdater updater,
            ITargetFilter targetFilter,
            ITargetPositionPredictor targetPositionPredictor,
            IAngleCalculator angleCalculator,
            AccuracyAdjuster accuracyAdjuster,
            IRotationMovementController rotationMovementController,
            FacingMinRangePositionValidator targetPositionValidator,
            AngleLimiter angleLimiter,
            CruiserSpecificFactories cruiserSpecificFactories,
            ITarget parent,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProvider,
            ICruiser enemyCruiser,
            ISoundKey firingSound = null,
            IAnimation barrelFiringAnimation = null)
        {
            Helper.AssertIsNotNull(
                updater,
                targetFilter,
                targetPositionPredictor,
                angleCalculator,
                accuracyAdjuster,
                rotationMovementController,
                targetPositionValidator,
                angleLimiter,
                cruiserSpecificFactories,
                parent,
                localBoostProviders,
                globalFireRateBoostProvider,
                enemyCruiser);

            Updater = updater;
            TargetFilter = targetFilter;
            TargetPositionPredictor = targetPositionPredictor;
            AngleCalculator = angleCalculator;
            AccuracyAdjuster = accuracyAdjuster;
            RotationMovementController = rotationMovementController;
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
    }
}
