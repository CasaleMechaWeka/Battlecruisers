using BattleCruisers.Projectiles.Stats;

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
		
        public IAngleCalculator CreateAngleCalculator(IFlightStats projectileFlightStats)
        {
            return new AngleCalculator(_angleHelper, projectileFlightStats);
        }

        public IAngleCalculator CreateArtilleryAngleCalculator(IFlightStats projectileFlightStats)
		{
            return new ArtilleryAngleCalculator(_angleHelper, projectileFlightStats);
		}

		public IAngleCalculator CreateMortarAngleCalculator(IFlightStats projectileFlightStats)
		{
            return new MortarAngleCalculator(_angleHelper, projectileFlightStats);
		}

		public IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees)
		{
            return new StaticAngleCalculator(_angleHelper, desiredAngleInDegrees);
		}
	}
}

