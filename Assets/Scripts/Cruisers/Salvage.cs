using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases offence build speed
    /// + Increases offence fire rate
    /// </summary>
    public class Salvage : Cruiser
    {
        public float offensivesBuildRateBoost;
        public float offenseFireRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(offensivesBuildRateBoost > 0);
            Assert.IsTrue(offenseFireRateBoost > 0);

            IBoostProvider offensivesBuildBoost = new BoostProvider(offensivesBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.OffensivesProviders.Add(offensivesBuildBoost);

            IBoostProvider offenseFireRate = new BoostProvider(offenseFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.OffenseFireRateBoostProviders.Add(offenseFireRate);
        }
    }
}




