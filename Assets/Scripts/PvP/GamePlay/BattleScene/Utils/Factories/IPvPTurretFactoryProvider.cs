using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public interface IPvPTurretFactoryProvider
    {
        ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
        IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        IAngleCalculatorFactory AngleCalculatorFactory { get; }
        IAngleLimiterFactory AngleLimiterFactory { get; }
        IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
    }
}