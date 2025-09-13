using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases naval factory build speed
    /// + Increases ship build speed
    /// </summary>
    public class Middlodon : Cruiser
    {
        public float navalFactoryBuildRateBoost;
        public float shipBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(navalFactoryBuildRateBoost > 0);
            Assert.IsTrue(shipBuildRateBoost > 0);

            IBoostProvider navalFactoryBoostProvider = new BoostProvider(navalFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.NavalFactoryProviders.Add(navalFactoryBoostProvider);

            IBoostProvider shipBoostProvider = new BoostProvider(shipBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.ShipProviders.Add(shipBoostProvider);
        }
    }
}


