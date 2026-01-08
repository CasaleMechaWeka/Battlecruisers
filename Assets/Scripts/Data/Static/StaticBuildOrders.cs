using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Static
{
    public static class StaticBuildOrders
    {
        public static ReadOnlyCollection<BuildingKey> Balanced = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,   //here we can insert offensives or other builidngs in BuildOrderFactory
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.StealthGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.DroneStation4
        });

        public static ReadOnlyCollection<BuildingKey> Boom = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.StealthGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.DroneStation4,
            null,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            null,
        });

        public static ReadOnlyCollection<BuildingKey> FortressPrime = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            StaticPrefabKeys.Buildings.StealthGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.DroneStation4,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.DroneStation8,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            StaticPrefabKeys.Buildings.ShieldGenerator,
            null,
            null,
        });

        public static ReadOnlyCollection<BuildingKey> LV032 = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            // LV032 Strategy: Massive expansion for Longbow+Raptor level with endless offensive opportunities
            // Starts with ship turret, then continues with boom economy and offensive capabilities

            // Core Economy Base (slots 0-9)
            StaticPrefabKeys.Buildings.SamSite,  // Slot 0: Build ship turret first
            StaticPrefabKeys.Buildings.DroneStation,     // Slot 1: Second drone station
            StaticPrefabKeys.Buildings.DroneStation,     // Slot 2: Third drone station
            StaticPrefabKeys.Buildings.GrapheneBarrier,     // Slot 3: Fourth drone station (basic economy)
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 4: First defense
            StaticPrefabKeys.Buildings.DroneStation4,    // Slot 5: Upgrade to tier 2 drones\
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 7: More defense
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 8: More tier 2
            StaticPrefabKeys.Buildings.LocalBooster,     // Slot 9: Attack/Defense boost

            // First Wave of Offensives (slots 10-19)
            null, null, null, null, null, null, null, null, null, null,

            // Mid-Game Scaling
            StaticPrefabKeys.Buildings.DroneStation4,    // Slot 20: Continue economy
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 21: Defense
            StaticPrefabKeys.Buildings.DroneStation6,    // Slot 22: Tier 3 drones
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 23: More defense
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 24: More tier 3
            StaticPrefabKeys.Buildings.DroneStation6,    // Slot 26: More tier 3
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 27: Defense
            StaticPrefabKeys.Buildings.DroneStation6,    // Slot 28: More tier 3
            StaticPrefabKeys.Buildings.LocalBooster,     // Slot 29: Boost

            // Second Wave of Offensives 
            null, null, null, null, null, null, null, null, null, null,

            // Late Game Power 
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 40: Max tier drones
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 41: Defense
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 42: More max tier
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 44: More max tier
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 45: Defense
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 46: More max tier
            StaticPrefabKeys.Buildings.LocalBooster,     // Slot 47: Boost
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 48: More max tier
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 49: Final defense

            // Third Wave of Offensives 
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null,
        });

        public static ReadOnlyCollection<BuildingKey> Rush = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            null,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
            StaticPrefabKeys.Buildings.DroneStation,
        });

        /// <summary>
        /// LV033 Strategy: Naval-focused defense with Salvage hull
        /// Salvage hull has exactly 12 slots for these buildings:
        /// 1-7: Economy/Defense foundation
        /// 8-10: Offensive buildings
        /// 11-12: Power buildings (Ultralisk + null for offensive requests)
        /// NOTE: Delete/replace operations must be done in SequencerLV033.prefab
        /// Units (Frigate, FlakTurtle, SiegeDestroyer, ArchonBattleship, AttackRIB)
        /// are produced by NavalFactory, not placed in slots.
        /// </summary>
        public static ReadOnlyCollection<BuildingKey> LV033 = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            // Slot 0: DroneStation (2Z)
            StaticPrefabKeys.Buildings.DroneStation,

            // Slot 1: DroneStation (2Z) - to be upgraded to 8Z by sequencer
            StaticPrefabKeys.Buildings.DroneStation,

            // Slot 2: ShieldGenerator
            StaticPrefabKeys.Buildings.ShieldGenerator,

            // Slot 3: DroneStation4 (4Z)
            StaticPrefabKeys.Buildings.DroneStation4,

            // Slot 4: MissilePod
            StaticPrefabKeys.Buildings.MissilePod,

            // Slot 5: ShieldGenerator
            StaticPrefabKeys.Buildings.ShieldGenerator,

            // Slot 6: CIWS
            StaticPrefabKeys.Buildings.CIWS,

            // Slot 7: MissilePod (another)
            StaticPrefabKeys.Buildings.MissilePod,

            // Slot 8: MissilePod (third)
            StaticPrefabKeys.Buildings.MissilePod,

            // Slot 9: NavalFactory (sequencer/chainbattle enemy cycles ships starting at Frigate; see AIManager.GetSequencerShipCycle)
            StaticPrefabKeys.Buildings.NavalFactory,

            // Slot 10: Ultralisk (ultra weapon)
            StaticPrefabKeys.Buildings.Ultralisk,

            // Slot 11+: Null slots for offensive requests to fill with additional buildings
            null,
            null, null, null, null, null, null, null, null, null,  // Slots 12-20
            null, null, null, null, null, null, null, null, null, null,  // Slots 21-30
            null, null, null, null, null, null, null, null, null, null,  // Slots 31-40
            null, null, null, null, null, null, null, null, null, null,  // Slots 41-50
            null, null, null, null, null, null, null, null, null, null,  // Slots 51-60
            null, null, null, null, null, null, null, null, null, null,  // Slots 61-70
            null, null, null, null, null, null, null, null, null, null,  // Slots 71-80
            null, null, null, null, null, null, null, null, null, null,  // Slots 81-90
            null, null, null, null, null, null, null, null, null, null,  // Slots 91-100
        });

    }
}
