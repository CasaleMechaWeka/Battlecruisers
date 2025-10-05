using BattleCruisers.Buildables.Boost;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases ultras build speed
    /// + Increases mast/structure build speed
    /// + Increases shield build speed
    /// + Increases shield recharge rate
    /// </summary>
    public class Axiom : Cruiser
    {
        public float ultrasBuildRateBoost;
        public float mastStructureBuildRateBoost;
        public float shieldBuildRateBoost;
        public float shieldRechargeRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(ultrasBuildRateBoost > 0);
            Assert.IsTrue(mastStructureBuildRateBoost > 0);
            Assert.IsTrue(shieldBuildRateBoost > 0);
            Assert.IsTrue(shieldRechargeRateBoost > 0);

            IBoostProvider ultrasBoost = new BoostProvider(ultrasBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.UltrasProviders.Add(ultrasBoost);

            IBoostProvider mastBoost = new BoostProvider(mastStructureBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.MastStructureProviders.Add(mastBoost);

            IBoostProvider buildRateBoost = new BoostProvider(shieldBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.ShieldsProviders.Add(buildRateBoost);

            IBoostProvider rechargeRateBoost = new BoostProvider(shieldRechargeRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.ShieldRechargeRateBoostProviders.Add(rechargeRateBoost);
        }
    }
}


