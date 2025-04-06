using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
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

        public override void Initialise(CruiserArgs args)
        {
            if (ApplicationModel.SelectedLevel == 38) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                airFactoryBuildRateBoost = SetUltraCruiserUtility(args, airFactoryBuildRateBoost);
                aircraftBuildRateBoost = SetUltraCruiserUtility(args, aircraftBuildRateBoost);
            }

            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircraftBuildRateBoost > 0);

            IBoostProvider factoryBoostProvider = new BoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(factoryBoostProvider);

            IBoostProvider aircraftBoostProvider = new BoostProvider(aircraftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);
        }
    }
}