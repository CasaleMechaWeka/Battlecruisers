namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IAngleCalculatorFactory
	{
		IAngleCalculator CreateAngleCalcultor();
		IAngleCalculator CreateArtilleryAngleCalcultor();
		IAngleCalculator CreateMortarAngleCalcultor();
		IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees);
	}
}
