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
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IBarrelControllerArgs
    {
        ITargetFilter TargetFilter { get; }
        ITargetPositionPredictor TargetPositionPredictor { get; }
        IAngleCalculator AngleCalculator { get; }
        IAttackablePositionFinder AttackablePositionFinder { get; }
        IAccuracyAdjuster AccuracyAdjuster { get; }
        IRotationMovementController RotationMovementController { get; }
        ITargetPositionValidator TargetPositionValidator { get; }
        IAngleLimiter AngleLimiter { get; }
        IFactoryProvider FactoryProvider { get; }
        ITarget Parent { get; }
        ISoundKey SpawnerSoundKey { get; }
        IObservableCollection<IBoostProvider> LocalBoostProviders { get; }
        IObservableCollection<IBoostProvider> GlobalFireRateBoostProviders { get; }
    }
}
