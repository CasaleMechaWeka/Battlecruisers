namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculatorFactory : IAngleCalculatorFactory
	{
        private readonly IAngleHelper _angleHelper;

        public AngleCalculatorFactory()
        {
            _angleHelper = new AngleHelper();
        }

        public IAngleHelper CreateAngleHelper()
        {
            return _angleHelper;
        }
		
        public IAngleCalculator CreateAngleCalculator()
        {
            return new AngleCalculator(_angleHelper);
        }

        public IAngleCalculator CreateArtilleryAngleCalculator()
		{
            return new ArtilleryAngleCalculator(_angleHelper);
		}

		public IAngleCalculator CreateMortarAngleCalculator()
		{
            return new MortarAngleCalculator(_angleHelper);
		}

		public IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees)
		{
            return new StaticAngleCalculator(_angleHelper, desiredAngleInDegrees);
		}
	}
}

