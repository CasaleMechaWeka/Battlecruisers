using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculatorFactory : IAngleCalculatorFactory
	{
        private readonly IAngleHelper _angleHelper;
        private readonly IAngleConverter _angleConverter;

        public AngleCalculatorFactory()
        {
            _angleHelper = new AngleHelper();
            _angleConverter = new AngleConverter();
        }

        public IAngleHelper CreateAngleHelper()
        {
            return _angleHelper;
        }
		
        public IAngleCalculator CreateAngleCalculator()
        {
            return new AngleCalculator(_angleHelper);
        }

        public IAngleCalculator CreateArtilleryAngleCalculator(IProjectileFlightStats projectileFlightStats)
		{
            return new ArtilleryAngleCalculator(_angleHelper, projectileFlightStats, _angleConverter);
		}

		public IAngleCalculator CreateMortarAngleCalculator(IProjectileFlightStats projectileFlightStats)
		{
            return new MortarAngleCalculator(_angleHelper, projectileFlightStats, _angleConverter);
		}

		public IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees)
		{
            return new StaticAngleCalculator(_angleHelper, desiredAngleInDegrees);
		}
	}
}

