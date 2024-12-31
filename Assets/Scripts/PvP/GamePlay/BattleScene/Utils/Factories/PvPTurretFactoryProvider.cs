using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPTurretFactoryProvider : IPvPTurretFactoryProvider
    {
        public IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        public IAngleCalculatorFactory AngleCalculatorFactory { get; }
        public IPvPAngleLimiterFactory AngleLimiterFactory { get; }
        public IPvPAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        public IPvPTargetPositionValidatorFactory TargetPositionValidatorFactory { get; }

        public PvPTurretFactoryProvider()
        {
            AccuracyAdjusterFactory = new AccuracyAdjusterFactory();
            AngleCalculatorFactory = new AngleCalculatorFactory();
            AngleLimiterFactory = new PvPAngleLimiterFactory();
            AttackablePositionFinderFactory = new PvPAttackablePositionFinderFactory();
            TargetPositionValidatorFactory = new PvPTargetPositionValidatorFactory();
        }
    }
}