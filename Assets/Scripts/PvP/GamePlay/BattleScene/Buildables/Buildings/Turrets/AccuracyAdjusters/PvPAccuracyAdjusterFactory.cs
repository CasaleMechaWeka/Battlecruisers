using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class PvPAccuracyAdjusterFactory : IPvPAccuracyAdjusterFactory
    {
        private const float TARGET_X_MARGIN_IN_M = 0.75f;
        private const float TARGET_Y_MARGIN_IN_M = 0.75f;

        public IPvPAccuracyAdjuster CreateDummyAdjuster()
        {
            return new PvPDummyAccuracyAdjuster();
        }

        public IPvPAccuracyAdjuster CreateHorizontalImpactProjectileAdjuster(IPvPAngleCalculator angleCalculator, IPvPTurretStats turretStats)
        {
            return
                CreateAccuracyAdjuster(
                    angleCalculator,
                    turretStats,
                    new PvPHorizontalTargetBoundsFinder(TARGET_X_MARGIN_IN_M));
        }

        public IPvPAccuracyAdjuster CreateVerticalImpactProjectileAdjuster(IPvPAngleCalculator angleCalculator, IPvPTurretStats turretStats)
        {
            return
                CreateAccuracyAdjuster(
                    angleCalculator,
                    turretStats,
                    new PvPVerticalTargetBoundsFinder(TARGET_Y_MARGIN_IN_M));
        }

        private IPvPAccuracyAdjuster CreateAccuracyAdjuster(
            IPvPAngleCalculator angleCalculator,
            IPvPTurretStats turretStats,
            IPvPTargetBoundsFinder targetBoundsFinder)
        {
            return
                new PvPAccuracyAdjuster(
                    targetBoundsFinder,
                    angleCalculator,
                    new PvPLinearRangeFinder(),
                    PvPRandomGenerator.Instance,
                    turretStats);
        }
    }
}
