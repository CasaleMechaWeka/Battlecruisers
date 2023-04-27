using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases shild build rate
    /// + Increases shield recharge rate
    /// </summary>
    public class PvPRaptor : PvPCruiser
    {
        public float shieldRechargeRateBoost;
        public float shieldBuildRateBoost;

        public override void Initialise(IPvPCruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(shieldRechargeRateBoost > 0);
            Assert.IsTrue(shieldBuildRateBoost > 0);

            IPvPBoostProvider rechargeRateBoost = FactoryProvider.BoostFactory.CreateBoostProvider(shieldRechargeRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders.Add(rechargeRateBoost);

            IPvPBoostProvider buildRateBoost = FactoryProvider.BoostFactory.CreateBoostProvider(shieldBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders.Add(buildRateBoost);
        }
    }
}