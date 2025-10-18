using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases tacticals build speed
    /// + SPECIALIZED: All buildings have massively inflated health but cost more to build
    /// </summary>
    public class TekGnosis : Cruiser
    {
        public float tacticalsBuildRateBoost;
        public float buildingHealthMultiplier;
        public float buildingBuildTimeMultiplier;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(tacticalsBuildRateBoost > 0);
            Assert.IsTrue(buildingHealthMultiplier > 0);
            Assert.IsTrue(buildingBuildTimeMultiplier > 0);

            IBoostProvider tacticalsBoost = new BoostProvider(tacticalsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.TacticalsProviders.Add(tacticalsBoost);

            // Building health boost - applies to all buildings
            IBoostProvider buildingHealthBoost = new BoostProvider(buildingHealthMultiplier);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingHealth.AllBuildingsProviders.Add(buildingHealthBoost);
        }
    }
}




