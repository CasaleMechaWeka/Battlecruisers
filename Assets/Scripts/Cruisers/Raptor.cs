using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Data;
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
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;
            if (applicationModel.SelectedLevel == 33) //This is where UltraCruiser Level is designated
            {
                SetUltraCruiserHealth(args);
                shieldRechargeRateBoost = SetUltraCruiserUtility(args, shieldRechargeRateBoost);
                shieldBuildRateBoost = SetUltraCruiserUtility(args, shieldBuildRateBoost);
            }
            base.Initialise(args);

            Assert.IsTrue(shieldRechargeRateBoost > 0);
            Assert.IsTrue(shieldBuildRateBoost > 0);

            IBoostProvider rechargeRateBoost = FactoryProvider.BoostFactory.CreateBoostProvider(shieldRechargeRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders.Add(rechargeRateBoost);

            IBoostProvider buildRateBoost = FactoryProvider.BoostFactory.CreateBoostProvider(shieldBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders.Add(buildRateBoost);
        }
    }
}