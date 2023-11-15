using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IPvPAccuracyAdjusterFactory
    {
        IPvPAccuracyAdjuster CreateDummyAdjuster();
        IPvPAccuracyAdjuster CreateHorizontalImpactProjectileAdjuster(IPvPAngleCalculator angleCalculator, IPvPTurretStats turretStats);
        IPvPAccuracyAdjuster CreateVerticalImpactProjectileAdjuster(IPvPAngleCalculator angleCalculator, IPvPTurretStats turretStats);
    }
}
