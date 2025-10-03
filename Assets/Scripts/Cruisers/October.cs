using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases aircraft factory build speed
    /// + SPECIALIZED: Stealth Generator builds nearly instantly with 1 drone, low health
    /// </summary>
    public class October : Cruiser
    {
        public float airFactoryBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);

            IBoostProvider airFactoryBoost = new BoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(airFactoryBoost);

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


