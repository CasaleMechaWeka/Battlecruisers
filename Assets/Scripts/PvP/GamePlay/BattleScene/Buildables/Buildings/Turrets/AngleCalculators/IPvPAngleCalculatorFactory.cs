using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IPvPAngleCalculatorFactory
    {
        IPvPAngleHelper CreateAngleHelper();
        IPvPAngleCalculator CreateAngleCalculator();
        IPvPAngleCalculator CreateArtilleryAngleCalculator(IPvPProjectileFlightStats projectileFlightStats);
        IPvPAngleCalculator CreateMortarAngleCalculator(IPvPProjectileFlightStats projectileFlightStats);
        IPvPAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees);
    }
}
