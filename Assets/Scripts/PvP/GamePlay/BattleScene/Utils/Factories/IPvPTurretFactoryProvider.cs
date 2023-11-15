using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public interface IPvPTurretFactoryProvider
    {
        IPvPAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        IPvPAngleCalculatorFactory AngleCalculatorFactory { get; }
        IPvPAngleLimiterFactory AngleLimiterFactory { get; }
        IPvPAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        IPvPTargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
    }
}