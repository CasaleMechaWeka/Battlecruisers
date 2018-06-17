using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class BarrelControllerArgs : IBarrelControllerArgs
    {
        public ITargetFilter TargetFilter { get; private set; }
        public ITargetPositionPredictor TargetPositionPredictor { get; private set; }
        public IAngleCalculator AngleCalculator { get; private set; }
        public IAccuracyAdjuster AccuracyAdjuster { get; private set; }
        public IRotationMovementController RotationMovementController { get; private set; }
        public IFactoryProvider FactoryProvider { get; private set; }
        public ITargetPositionValidator TargetPositionValidator { get; private set; }
        public IAngleLimiter AngleLimiter { get; private set; }
        public ITarget Parent { get; private set; }
        public ISoundKey SpawnerSoundKey { get; private set; }
        public IObservableCollection<IBoostProvider> LocalBoostProviders { get; private set; }

        public BarrelControllerArgs(
            ITargetFilter targetFilter,
            ITargetPositionPredictor targetPositionPredictor,
            IAngleCalculator angleCalculator,
            IAccuracyAdjuster accuracyAdjuster,
            IRotationMovementController rotationMovementController,
			ITargetPositionValidator targetPositionValidator,
            IAngleLimiter angleLimiter,
            IFactoryProvider factoryProvider,
            ITarget parent,
            ISoundKey firingSound = null,
            IObservableCollection<IBoostProvider> localBoostProviders = null)
        {
            Helper.AssertIsNotNull(
                targetFilter, 
                targetPositionPredictor, 
                angleCalculator, 
                accuracyAdjuster, 
                rotationMovementController, 
                targetPositionValidator,
                angleLimiter,
                factoryProvider,
                parent);

            TargetFilter = targetFilter;
            TargetPositionPredictor = targetPositionPredictor;
            AngleCalculator = angleCalculator;
            AccuracyAdjuster = accuracyAdjuster;
            RotationMovementController = rotationMovementController;
            FactoryProvider = factoryProvider;
            AngleLimiter = angleLimiter;
            TargetPositionValidator = targetPositionValidator;
            Parent = parent;
            SpawnerSoundKey = firingSound;
            LocalBoostProviders = localBoostProviders;
        }
    }
}
