using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Decreases naval factory build speed
    /// + Increases ship build speed
    /// + Decreases aircraft factory build speed
    /// + Increases aircraft build speed
    /// </summary>
    public class Flea : Cruiser
    {
        public float navalFactoryBuildRateBoost;
        public float shipBuildRateBoost;
        public float airFactoryBuildRateBoost;
        [FormerlySerializedAs("aircarftBuildRateBoost")] public float aircraftBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 37) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                navalFactoryBuildRateBoost = SetUltraCruiserUtility(args, navalFactoryBuildRateBoost);
                shipBuildRateBoost = SetUltraCruiserUtility(args, shipBuildRateBoost);
                airFactoryBuildRateBoost = SetUltraCruiserUtility(args, airFactoryBuildRateBoost);
                aircraftBuildRateBoost = SetUltraCruiserUtility(args, aircraftBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(navalFactoryBuildRateBoost > 0);
            Assert.IsTrue(shipBuildRateBoost > 0);
            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircraftBuildRateBoost > 0);

            IBoostProvider navalFactoryBoostProvider = new BoostProvider(navalFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders.Add(navalFactoryBoostProvider);

            IBoostProvider shipBoostProvider = new BoostProvider(shipBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.ShipProviders.Add(shipBoostProvider);

            IBoostProvider aircraftFactoryBoostProvider = new BoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(aircraftFactoryBoostProvider);

            IBoostProvider aircraftBoostProvider = new BoostProvider(aircraftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoostProvider);
        }
    }
}