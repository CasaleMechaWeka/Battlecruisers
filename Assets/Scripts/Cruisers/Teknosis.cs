using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases tacticals build speed
    /// + Increases stealth/shield generators build speed
    /// </summary>
    public class Teknosis : Cruiser
    {
        public float tacticalsBuildRateBoost;
        public float shieldBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(tacticalsBuildRateBoost > 0);
            Assert.IsTrue(shieldBuildRateBoost > 0);

            IBoostProvider tacticalsBoost = new BoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(tacticalsBoost);

            IBoostProvider shieldBoost = new BoostProvider(shieldBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders.Add(shieldBoost);
        }
    }
}


