using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases all buildings health
    /// + Increases shield build speed
    /// </summary>
    public class EndlessWall : Cruiser
    {
        public float buildingHealthBoost;
        public float shieldBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(buildingHealthBoost > 0);
            Assert.IsTrue(shieldBuildRateBoost > 0);

            IBoostProvider healthBoost = new BoostProvider(buildingHealthBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingHealth.AllBuildingsProviders.Add(healthBoost);

            IBoostProvider shieldsBoost = new BoostProvider(shieldBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders.Add(shieldsBoost);
        }
    }
}


