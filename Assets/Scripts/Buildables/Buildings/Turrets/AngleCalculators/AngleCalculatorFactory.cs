using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculatorFactory : IAngleCalculatorFactory
    {
        private readonly IAngleConverter _angleConverter;

        public AngleCalculatorFactory()
        {
            _angleConverter = new AngleConverter();
        }

        public IAngleCalculator CreateAngleCalculator()
        {
            return new AngleCalculator();
        }

        public IAngleCalculator CreateArtilleryAngleCalculator(IProjectileFlightStats projectileFlightStats)
        {
            return new ArtilleryAngleCalculator(_angleConverter, projectileFlightStats);
        }

        public IAngleCalculator CreateMortarAngleCalculator(IProjectileFlightStats projectileFlightStats)
        {
            return new MortarAngleCalculator(_angleConverter, projectileFlightStats);
        }

        public IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees)
        {
            return new StaticAngleCalculator(desiredAngleInDegrees);
        }
    }
}

