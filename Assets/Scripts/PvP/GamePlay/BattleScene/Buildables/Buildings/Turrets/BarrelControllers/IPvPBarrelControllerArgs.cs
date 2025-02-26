using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IPvPBarrelControllerArgs
    {
        IUpdater Updater { get; }
        ITargetFilter TargetFilter { get; }
        ITargetPositionPredictor TargetPositionPredictor { get; }
        IAngleCalculator AngleCalculator { get; }
        IPvPAttackablePositionFinder AttackablePositionFinder { get; }
        IAccuracyAdjuster AccuracyAdjuster { get; }
        IRotationMovementController RotationMovementController { get; }
        ITargetPositionValidator TargetPositionValidator { get; }
        IAngleLimiter AngleLimiter { get; }
        IPvPFactoryProvider FactoryProvider { get; }
        IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        ITarget Parent { get; }
        ISoundKey SpawnerSoundKey { get; }
        ObservableCollection<IBoostProvider> LocalBoostProviders { get; }
        ObservableCollection<IBoostProvider> GlobalFireRateBoostProviders { get; }
        IPvPAnimation BarrelFiringAnimation { get; }
        IPvPCruiser EnemyCruiser { get; }
    }
}
