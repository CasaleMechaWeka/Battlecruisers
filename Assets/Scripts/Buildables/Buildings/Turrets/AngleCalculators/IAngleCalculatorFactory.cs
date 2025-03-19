using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public interface IAngleCalculatorFactory
	{
		AngleHelper CreateAngleHelper();
		IAngleCalculator CreateAngleCalculator();
		IAngleCalculator CreateArtilleryAngleCalculator(IProjectileFlightStats projectileFlightStats);
		IAngleCalculator CreateMortarAngleCalculator(IProjectileFlightStats projectileFlightStats);
		IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees);
	}
}
