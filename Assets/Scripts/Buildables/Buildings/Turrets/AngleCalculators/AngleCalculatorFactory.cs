namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculatorFactory : IAngleCalculatorFactory
	{
		public IAngleCalculator CreateAngleCalcultor()
		{
			return new AngleCalculator();
		}

		public IAngleCalculator CreateArtilleryAngleCalcultor()
		{
			return new ArtilleryAngleCalculator();
		}

		public IAngleCalculator CreateMortarAngleCalcultor()
		{
			return new MortarAngleCalculator();
		}

		public IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees)
		{
			return new StaticAngleCalculator(desiredAngleInDegrees);
		}
	}
}

