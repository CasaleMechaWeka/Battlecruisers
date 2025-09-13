using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases all buildings build speed
    /// + Increases ship build speed
    /// </summary>
    public class Arkdeso : Cruiser
    {
        public float allBuildingsBuildRateBoost;
        public float shipsBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(allBuildingsBuildRateBoost > 0);
            Assert.IsTrue(shipsBuildRateBoost > 0);

            IBoostProvider allBuildings = new BoostProvider(allBuildingsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AllBuildingsProviders.Add(allBuildings);

            IBoostProvider ships = new BoostProvider(shipsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.ShipProviders.Add(ships);
        }
    }
}


