using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases shild build rate
    /// + Increases shield recharge rate
    /// </summary>
    public class Raptor : Cruiser
    {
        public float shieldRechargeRateBoost;
        public float shieldBuildRateBoost;

        public override void Initialise(ICruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(shieldRechargeRateBoost > 0);
            Assert.IsTrue(shieldBuildRateBoost > 0);

            IBoostProvider rechargeRateBoost = FactoryProvider.BoostFactory.CreateBoostProvider(shieldRechargeRateBoost);
            FactoryProvider.GlobalBoostProviders.ShieldRechargeRateBoostProviders.Add(rechargeRateBoost);

            IBoostProvider buildRateBoost = FactoryProvider.BoostFactory.CreateBoostProvider(shieldBuildRateBoost);
            FactoryProvider.GlobalBoostProviders.ShieldRechargeRateBoostProviders.Add(buildRateBoost);
        }
    }
}