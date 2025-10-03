using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases tacticals build speed
    /// + Increases stealth/shield generators build speed
    /// + SPECIALIZED: Stealth Generator builds nearly instantly with 1 drone
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

            // SPECIALIZED: Stealth Generator - near-instant build with 1 drone, normal health
            var stealthModifiers = new SpecializedBuildableModifiers(
                buildTimeMultiplier: 0.05f,        // 5% of normal build time
                droneRequirementOverride: 1,       // Only needs 1 drone
                healthMultiplier: 1.0f             // Normal health
            );
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["StealthGenerator"] = stealthModifiers;
        }
    }
}


