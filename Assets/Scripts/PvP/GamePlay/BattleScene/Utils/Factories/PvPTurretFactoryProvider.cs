using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPTurretFactoryProvider : IPvPTurretFactoryProvider
    {
        public IPvPAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        public IPvPAngleCalculatorFactory AngleCalculatorFactory { get; }
        public IPvPAngleLimiterFactory AngleLimiterFactory { get; }
        public IPvPAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        public IPvPTargetPositionValidatorFactory TargetPositionValidatorFactory { get; }

        public PvPTurretFactoryProvider()
        {
            AccuracyAdjusterFactory = new PvPAccuracyAdjusterFactory();
            AngleCalculatorFactory = new PvPAngleCalculatorFactory();
            AngleLimiterFactory = new PvPAngleLimiterFactory();
            AttackablePositionFinderFactory = new PvPAttackablePositionFinderFactory();
            TargetPositionValidatorFactory = new PvPTargetPositionValidatorFactory();
        }
    }
}