using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases all buildings build speed
    /// + Increases aircraft build speed
    /// </summary>
    public class Essex : Cruiser
    {
        public float allBuildingsBuildRateBoost;
        public float aircraftBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(allBuildingsBuildRateBoost > 0);
            Assert.IsTrue(aircraftBuildRateBoost > 0);

            IBoostProvider allBuildingsBoost = new BoostProvider(allBuildingsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AllBuildingsProviders.Add(allBuildingsBoost);

            IBoostProvider aircraftBoost = new BoostProvider(aircraftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoost);
        }
    }
}


