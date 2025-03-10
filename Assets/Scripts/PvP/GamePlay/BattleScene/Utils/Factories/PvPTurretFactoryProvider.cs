using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public class PvPTurretFactoryProvider : ITurretFactoryProvider
    {
        public IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        public IAngleCalculatorFactory AngleCalculatorFactory { get; }
        public IAngleLimiterFactory AngleLimiterFactory { get; }
        public IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        public ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }

        public PvPTurretFactoryProvider()
        {
            AccuracyAdjusterFactory = new AccuracyAdjusterFactory();
            AngleCalculatorFactory = new AngleCalculatorFactory();
            AngleLimiterFactory = new AngleLimiterFactory();
            AttackablePositionFinderFactory = new PvPAttackablePositionFinderFactory();
            TargetPositionValidatorFactory = new PvPTargetPositionValidatorFactory();
        }
    }
}