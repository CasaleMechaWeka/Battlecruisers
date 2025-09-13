using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases all building health
    /// + Increases shield recharge rate
    /// </summary>
    public class Orac : Cruiser
    {
        public float buildingHealthBoost;
        public float shieldRechargeRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(buildingHealthBoost > 0);
            Assert.IsTrue(shieldRechargeRateBoost > 0);

            IBoostProvider healthBoost = new BoostProvider(buildingHealthBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingHealth.AllBuildingsProviders.Add(healthBoost);

            IBoostProvider rechargeBoost = new BoostProvider(shieldRechargeRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders.Add(rechargeBoost);
        }
    }
}


