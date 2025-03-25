using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases shield build rate
    /// + Increases shield recharge rate
    /// </summary>
    public class Raptor : Cruiser
    {
        public float shieldRechargeRateBoost;
        public float shieldBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            if (ApplicationModel.SelectedLevel is 33 or 40) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                shieldRechargeRateBoost = SetUltraCruiserUtility(args, shieldRechargeRateBoost);
                shieldBuildRateBoost = SetUltraCruiserUtility(args, shieldBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(shieldRechargeRateBoost > 0);
            Assert.IsTrue(shieldBuildRateBoost > 0);

            IBoostProvider rechargeRateBoost = new BoostProvider(shieldRechargeRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders.Add(rechargeRateBoost);

            IBoostProvider buildRateBoost = new BoostProvider(shieldBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders.Add(buildRateBoost);
        }
    }
}