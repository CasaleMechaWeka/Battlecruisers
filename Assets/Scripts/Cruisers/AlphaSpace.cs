using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Decreases aircraft build speed
    /// + Increases missile/rocket fire rate
    /// </summary>
    public class AlphaSpace : Cruiser
    {
        public float aircraftBuildRateBoost;
        public float rocketFireRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(aircraftBuildRateBoost > 0);
            Assert.IsTrue(rocketFireRateBoost > 0);

            IBoostProvider aircraftBoost = new BoostProvider(aircraftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoost);

            IBoostProvider rocketsFire = new BoostProvider(rocketFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders.Add(rocketsFire);
        }
    }
}


