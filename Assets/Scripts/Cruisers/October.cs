using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases stealth generator build speed
    /// + Increases aircraft factory build speed
    /// </summary>
    public class October : Cruiser
    {
        public float stealthGeneratorBuildRateBoost;
        public float airFactoryBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(stealthGeneratorBuildRateBoost > 0);
            Assert.IsTrue(airFactoryBuildRateBoost > 0);

            IBoostProvider stealthBoost = new BoostProvider(stealthGeneratorBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(stealthBoost);

            IBoostProvider airFactoryBoost = new BoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(airFactoryBoost);
        }
    }
}


