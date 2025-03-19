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
            return new GravityAffectedAngleCalculator(_angleConverter, projectileFlightStats, false);
        }

        public IAngleCalculator CreateMortarAngleCalculator(IProjectileFlightStats projectileFlightStats)
        {
            return new GravityAffectedAngleCalculator(_angleConverter, projectileFlightStats, true);
        }

        public IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees)
        {
            return new StaticAngleCalculator(desiredAngleInDegrees);
        }
    }
}

