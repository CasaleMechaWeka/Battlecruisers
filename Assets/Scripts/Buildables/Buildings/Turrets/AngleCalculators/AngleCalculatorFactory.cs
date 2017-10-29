namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculatorFactory : IAngleCalculatorFactory
	{
		public IAngleCalculator CreateAngleCalculator()
		{
			return new AngleCalculator();
		}

		public IAngleCalculator CreateArtilleryAngleCalculator()
		{
			return new ArtilleryAngleCalculator();
		}

		public IAngleCalculator CreateMortarAngleCalculator()
		{
			return new MortarAngleCalculator();
		}

		public IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees)
		{
			return new StaticAngleCalculator(desiredAngleInDegrees);
		}
	}
}

