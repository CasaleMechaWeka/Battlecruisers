using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases rocket buildings fire rate
    /// + Increases rocket buildings build speed
    /// + SPECIALIZED: Stealth Generator builds nearly instantly with 1 drone, low health
    /// </summary>
    public class Zumwalt : Cruiser
    {
        public float rocketFireRateBoost;
        public float rocketBuildingsBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(rocketFireRateBoost > 0);
            Assert.IsTrue(rocketBuildingsBuildRateBoost > 0);

            IBoostProvider fireRateBoostProvider = new BoostProvider(rocketFireRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.RocketBuildingsFireRateBoostProviders.Add(fireRateBoostProvider);

            IBoostProvider rocketBuildRateProvider = new BoostProvider(rocketBuildingsBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.RocketBuildingsProviders.Add(rocketBuildRateProvider);

            // SPECIALIZED: Stealth Generator - near-instant build with 1 drone, fragile
            var stealthModifiers = new SpecializedBuildableModifiers(
                buildTimeMultiplier: 0.05f,        // 5% of normal build time (near-instant)
                droneRequirementOverride: 1,       // Only needs 1 drone
                healthMultiplier: 0.05f            // 5% health (very fragile)
            );
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["StealthGenerator"] = stealthModifiers;
        }
    }
}


