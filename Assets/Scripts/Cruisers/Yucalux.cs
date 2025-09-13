using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases air factory build speed
    /// + Increases aircraft build speed
    /// </summary>
    public class Yucalux : Cruiser
    {
        public float airFactoryBuildRateBoost;
        public float aircraftBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircraftBuildRateBoost > 0);

            IBoostProvider airFactoryBoost = new BoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(airFactoryBoost);

            IBoostProvider aircraftBoost = new BoostProvider(aircraftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoost);
        }
    }
}


