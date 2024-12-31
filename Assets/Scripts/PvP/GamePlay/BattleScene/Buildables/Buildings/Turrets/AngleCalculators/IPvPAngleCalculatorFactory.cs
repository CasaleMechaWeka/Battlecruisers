using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IPvPAngleCalculatorFactory
    {
        IAngleHelper CreateAngleHelper();
        IAngleCalculator CreateAngleCalculator();
        IAngleCalculator CreateArtilleryAngleCalculator(IProjectileFlightStats projectileFlightStats);
        IAngleCalculator CreateMortarAngleCalculator(IProjectileFlightStats projectileFlightStats);
        IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees);
    }
}
