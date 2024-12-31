using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public class PvPAngleCalculatorFactory : IPvPAngleCalculatorFactory
    {
        private readonly IAngleHelper _angleHelper;
        private readonly IAngleConverter _angleConverter;

        public PvPAngleCalculatorFactory()
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

        public IAngleCalculator CreateArtilleryAngleCalculator(IPvPProjectileFlightStats projectileFlightStats)
        {
            return new PvPArtilleryAngleCalculator(_angleHelper, _angleConverter, projectileFlightStats);
        }

        public IAngleCalculator CreateMortarAngleCalculator(IPvPProjectileFlightStats projectileFlightStats)
        {
            return new PvPMortarAngleCalculator(_angleHelper, _angleConverter, projectileFlightStats);
        }

        public IAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees)
        {
            return new StaticAngleCalculator(_angleHelper, desiredAngleInDegrees);
        }
    }
}

