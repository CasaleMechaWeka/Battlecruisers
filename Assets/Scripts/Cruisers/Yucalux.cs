using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    /// <summary>
    /// Perks:
    /// + Increases air factory build speed
    /// + Increases aircraft build speed
    /// + SPECIALIZED: All aircraft have 50% health (fragile but fast to produce)
    /// </summary>
    public class Yucalux : Cruiser
    {
        public float airFactoryBuildRateBoost;
        public float aircraftBuildRateBoost;

        public override void Initialise(CruiserArgs args)
        {
            base.Initialise(args);

            Assert.IsTrue(airFactoryBuildRateBoost > 0);
            Assert.IsTrue(aircraftBuildRateBoost > 0);

            IBoostProvider airFactoryBoost = new BoostProvider(airFactoryBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.BuildingBuildRate.AirFactoryProviders.Add(airFactoryBoost);

            IBoostProvider aircraftBoost = new BoostProvider(aircraftBuildRateBoost);
            CruiserSpecificFactories.GlobalBoostProviders.UnitBuildRate.AircraftProviders.Add(aircraftBoost);

            // SPECIALIZED: All aircraft are fragile (50% health) but build faster
            // This creates a glass cannon air force strategy
            var aircraftModifiers = new SpecializedBuildableModifiers(
                buildTimeMultiplier: 1.0f,         // Normal build time (already boosted above)
                droneRequirementOverride: 0,       // No override, use default
                healthMultiplier: 0.5f             // 50% health (fragile)
            );
            
            // Apply to all aircraft types
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["Fighter"] = aircraftModifiers;
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["Bomber"] = aircraftModifiers;
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["MissileFighter"] = aircraftModifiers;
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["GunShip"] = aircraftModifiers;
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["StratBomber"] = aircraftModifiers;
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["Broadsword"] = aircraftModifiers;
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["SteamCopter"] = aircraftModifiers;
            CruiserSpecificFactories.GlobalBoostProviders.SpecializedBuildableBoosts["Kamikazoo"] = aircraftModifiers;
        }
    }
}


