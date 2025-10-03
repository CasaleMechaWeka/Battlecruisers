using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases rocket buildings fire rate
    /// + Increases rocket buildings build speed
    /// + Starts with fog of war active (set startsWithFogOfWar in Inspector)
    /// </summary>
    public class Zumwalt : Cruiser
    {
        public float rocketFireRateBoost;
        public float rocketBuildingsBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(rocketFireRateBoost > 0);
            Assert.IsTrue(rocketBuildingsBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = new BoostProvider(rocketFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider rocketBuildRateProvider = new BoostProvider(rocketBuildingsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.RocketBuildingsProviders.Add(rocketBuildRateProvider);
        }
    }
}


