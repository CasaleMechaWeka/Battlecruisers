using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases aircraft factory build speed
    /// + Starts with fog of war active (set startsWithFogOfWar in Inspector)
    /// </summary>
    public class October : Cruiser
    {
        public float airFactoryBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);

            IBoostProvider airFactoryBoost = new BoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(airFactoryBoost);
        }
    }
}


