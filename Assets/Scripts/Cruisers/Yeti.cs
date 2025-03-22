using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{

    /// <summary>
    /// Perks:
    /// + Increases build speed.
    /// </summary>
    /// 
    public class Yeti : Cruiser
    {
        public float buildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(buildRateBoost > 0);

            IBoostProvider boostProvider = new BoostProvider(buildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AllBuildingsProviders.Add(boostProvider);

            IBoostProvider aircraftBoostProvider = new BoostProvider(buildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);

            IBoostProvider shipBoostProvider = new BoostProvider(buildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.ShipProviders.Add(shipBoostProvider);
        }
    }
}
