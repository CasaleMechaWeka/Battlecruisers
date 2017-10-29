namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IAngleCalculatorFactory
	{
		IAngleCalculator CreateAngleCalculator();
		IAngleCalculator CreateArtilleryAngleCalculator();
		IAngleCalculator CreateMortarAngleCalculator();
		IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees);
	}
}
