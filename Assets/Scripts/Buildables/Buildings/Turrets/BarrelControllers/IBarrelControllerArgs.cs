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
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.Factories;
using System.Collections.ObjectModel;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IBarrelControllerArgs
    {
        IUpdater Updater { get; }
        ITargetFilter TargetFilter { get; }
        ITargetPositionPredictor TargetPositionPredictor { get; }
        IAngleCalculator AngleCalculator { get; }
        IAttackablePositionFinder AttackablePositionFinder { get; }
        IAccuracyAdjuster AccuracyAdjuster { get; }
        IRotationMovementController RotationMovementController { get; }
        ITargetPositionValidator TargetPositionValidator { get; }
        IAngleLimiter AngleLimiter { get; }
        IFactoryProvider FactoryProvider { get; }
        ICruiserSpecificFactories CruiserSpecificFactories { get; }
        ITarget Parent { get; }
        ISoundKey SpawnerSoundKey { get; }
        ObservableCollection<IBoostProvider> LocalBoostProviders { get; }
        ObservableCollection<IBoostProvider> GlobalFireRateBoostProviders { get; }
    }
}
