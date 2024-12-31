using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public interface IPvPTurretFactoryProvider
    {
        IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        IAngleCalculatorFactory AngleCalculatorFactory { get; }
        IPvPAngleLimiterFactory AngleLimiterFactory { get; }
        IPvPAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        IPvPTargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
    }
}