namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IAngleCalculatorFactory
	{
        IAngleHelper CreateAngleHelper();
		IAngleCalculator CreateAngleCalculator();
		IAngleCalculator CreateArtilleryAngleCalculator();
		IAngleCalculator CreateMortarAngleCalculator();
		IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees);
	}
}
