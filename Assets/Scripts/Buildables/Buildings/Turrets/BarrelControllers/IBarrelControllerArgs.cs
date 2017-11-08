using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IBarrelControllerArgs
    {
        ITargetFilter TargetFilter { get; }
        ITargetPositionPredictor TargetPositionPredictor { get; }
        IAngleCalculator AngleCalculator { get; }
        IAccuracyAdjuster AccuracyAdjuster { get; }
        IRotationMovementController RotationMovementController { get; }
        ITargetPositionValidator TargetPositionValidator { get; }
        IAngleLimiter AngleLimiter { get; }
        IFactoryProvider FactoryProvider { get; }
    }
}
