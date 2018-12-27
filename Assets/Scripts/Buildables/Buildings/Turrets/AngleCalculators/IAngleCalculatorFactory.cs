using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IAngleCalculatorFactory
	{
        IAngleHelper CreateAngleHelper();
		IAngleCalculator CreateAngleCalculator(IFlightStats projectileFlightStats);
		IAngleCalculator CreateArtilleryAngleCalculator(IFlightStats projectileFlightStats);
		IAngleCalculator CreateMortarAngleCalculator(IFlightStats projectileFlightStats);
		IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees);
	}
}
