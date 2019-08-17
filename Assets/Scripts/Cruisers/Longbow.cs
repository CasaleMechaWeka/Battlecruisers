using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases air factory build speed
    /// + Increases aircraft build speed
    /// </summary>
    public class Longbow : Cruiser
    {
        public float airFactoryBuildRateBoost;
        public float aircarftBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircarftBuildRateBoost > 0);

            IBoostProvider factoryBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(factoryBoostProvider);

            IBoostProvider aircraftBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(aircarftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);
        }
    }
}