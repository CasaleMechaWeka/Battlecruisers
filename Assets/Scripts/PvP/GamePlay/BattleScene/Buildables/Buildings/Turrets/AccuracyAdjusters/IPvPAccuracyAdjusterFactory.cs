using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IPvPAccuracyAdjusterFactory
    {
        IPvPAccuracyAdjuster CreateDummyAdjuster();
        IPvPAccuracyAdjuster CreateHorizontalImpactProjectileAdjuster(IAngleCalculator angleCalculator, ITurretStats turretStats);
        IPvPAccuracyAdjuster CreateVerticalImpactProjectileAdjuster(IAngleCalculator angleCalculator, ITurretStats turretStats);
    }
}
