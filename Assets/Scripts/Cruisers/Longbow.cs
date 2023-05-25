using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

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
        [FormerlySerializedAs("aircarftBuildRateBoost")] public float aircraftBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircraftBuildRateBoost > 0);

            IBoostProvider factoryBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(factoryBoostProvider);

            IBoostProvider aircraftBoostProvider = FactoryProvider.BoostFactory.CreateBoostProvider(aircraftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);
        }
    }
}