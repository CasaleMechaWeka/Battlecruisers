using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public class PvPAngleCalculatorFactory : IPvPAngleCalculatorFactory
    {
        private readonly IAngleHelper _angleHelper;
        private readonly IPvPAngleConverter _angleConverter;

        public PvPAngleCalculatorFactory()
        {
            _angleHelper = new AngleHelper();
            _angleConverter = new PvPAngleConverter();
        }

        public IAngleHelper CreateAngleHelper()
        {
            return _angleHelper;
        }

        public IPvPAngleCalculator CreateAngleCalculator()
        {
            return new PvPAngleCalculator(_angleHelper);
        }

        public IPvPAngleCalculator CreateArtilleryAngleCalculator(IPvPProjectileFlightStats projectileFlightStats)
        {
            return new PvPArtilleryAngleCalculator(_angleHelper, _angleConverter, projectileFlightStats);
        }

        public IPvPAngleCalculator CreateMortarAngleCalculator(IPvPProjectileFlightStats projectileFlightStats)
        {
            return new PvPMortarAngleCalculator(_angleHelper, _angleConverter, projectileFlightStats);
        }

        public IPvPAngleCalculator CreateStaticAngleCalculator(float desiredAngleInDegrees)
        {
            return new PvPStaticAngleCalculator(_angleHelper, desiredAngleInDegrees);
        }
    }
}

