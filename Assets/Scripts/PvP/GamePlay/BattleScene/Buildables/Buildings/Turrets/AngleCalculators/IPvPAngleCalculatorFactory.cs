using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IPvPAngleCalculatorFactory
    {
        IAngleHelper CreateAngleHelper();
        IAngleCalculator CreateAngleCalculator();
        IAngleCalculator CreateArtilleryAngleCalculator(IPvPProjectileFlightStats projectileFlightStats);
        IAngleCalculator CreateMortarAngleCalculator(IPvPProjectileFlightStats projectileFlightStats);
        IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees);
    }
}
