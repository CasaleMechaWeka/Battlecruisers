using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IPvPBarrelControllerArgs
    {
        IPvPUpdater Updater { get; }
        ITargetFilter TargetFilter { get; }
        IPvPTargetPositionPredictor TargetPositionPredictor { get; }
        IAngleCalculator AngleCalculator { get; }
        IPvPAttackablePositionFinder AttackablePositionFinder { get; }
        IAccuracyAdjuster AccuracyAdjuster { get; }
        IPvPRotationMovementController RotationMovementController { get; }
        IPvPTargetPositionValidator TargetPositionValidator { get; }
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
