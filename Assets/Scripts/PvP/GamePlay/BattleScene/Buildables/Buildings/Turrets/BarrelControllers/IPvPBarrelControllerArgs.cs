using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IPvPBarrelControllerArgs
    {
        IPvPUpdater Updater { get; }
        IPvPTargetFilter TargetFilter { get; }
        IPvPTargetPositionPredictor TargetPositionPredictor { get; }
        IPvPAngleCalculator AngleCalculator { get; }
        IPvPAttackablePositionFinder AttackablePositionFinder { get; }
        IPvPAccuracyAdjuster AccuracyAdjuster { get; }
        IPvPRotationMovementController RotationMovementController { get; }
        IPvPTargetPositionValidator TargetPositionValidator { get; }
        IPvPAngleLimiter AngleLimiter { get; }
        IPvPFactoryProvider FactoryProvider { get; }
        IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        IPvPTarget Parent { get; }
        IPvPSoundKey SpawnerSoundKey { get; }
        ObservableCollection<IPvPBoostProvider> LocalBoostProviders { get; }
        ObservableCollection<IPvPBoostProvider> GlobalFireRateBoostProviders { get; }
        IPvPAnimation BarrelFiringAnimation { get; }
        IPvPCruiser EnemyCruiser { get; }
    }
}
