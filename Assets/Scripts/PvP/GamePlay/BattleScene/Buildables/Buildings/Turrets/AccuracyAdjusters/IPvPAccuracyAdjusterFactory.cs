using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IPvPAccuracyAdjusterFactory
    {
        IAccuracyAdjuster CreateDummyAdjuster();
        IAccuracyAdjuster CreateHorizontalImpactProjectileAdjuster(IAngleCalculator angleCalculator, ITurretStats turretStats);
        IAccuracyAdjuster CreateVerticalImpactProjectileAdjuster(IAngleCalculator angleCalculator, ITurretStats turretStats);
    }
}
