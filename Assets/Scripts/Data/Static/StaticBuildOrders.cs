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
            StaticPrefabKeys.Buildings.AntiShipTurret,  // Slot 0: Build ship turret first
            StaticPrefabKeys.Buildings.DroneStation,     // Slot 1: Second drone station
            StaticPrefabKeys.Buildings.DroneStation,     // Slot 2: Third drone station
            StaticPrefabKeys.Buildings.DroneStation,     // Slot 3: Fourth drone station (basic economy)
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 4: First defense
            StaticPrefabKeys.Buildings.DroneStation4,    // Slot 5: Upgrade to tier 2 drones
            StaticPrefabKeys.Buildings.StealthGenerator, // Slot 6: Stealth for ChainBattle
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 7: More defense
            StaticPrefabKeys.Buildings.DroneStation4,    // Slot 8: More tier 2
            StaticPrefabKeys.Buildings.LocalBooster,     // Slot 9: Attack/Defense boost

            // First Wave of Offensives (slots 10-19)
            null, null, null, null, null, null, null, null, null, null,

            // Mid-Game Scaling (slots 20-29)
            StaticPrefabKeys.Buildings.DroneStation4,    // Slot 20: Continue economy
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 21: Defense
            StaticPrefabKeys.Buildings.DroneStation6,    // Slot 22: Tier 3 drones
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 23: More defense
            StaticPrefabKeys.Buildings.DroneStation6,    // Slot 24: More tier 3
            StaticPrefabKeys.Buildings.StealthGenerator, // Slot 25: More stealth
            StaticPrefabKeys.Buildings.DroneStation6,    // Slot 26: More tier 3
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 27: Defense
            StaticPrefabKeys.Buildings.DroneStation6,    // Slot 28: More tier 3
            StaticPrefabKeys.Buildings.LocalBooster,     // Slot 29: Boost

            // Second Wave of Offensives (slots 30-39)
            null, null, null, null, null, null, null, null, null, null,

            // Late Game Power (slots 40-49)
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 40: Max tier drones
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 41: Defense
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 42: More max tier
            StaticPrefabKeys.Buildings.StealthGenerator, // Slot 43: Stealth
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 44: More max tier
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 45: Defense
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 46: More max tier
            StaticPrefabKeys.Buildings.LocalBooster,     // Slot 47: Boost
            StaticPrefabKeys.Buildings.DroneStation8,    // Slot 48: More max tier
            StaticPrefabKeys.Buildings.ShieldGenerator,  // Slot 49: Final defense

            // Third Wave of Offensives (slots 50-59)
            null, null, null, null, null, null, null, null, null, null,

            // Endless Offensive Capability (slots 60-79)
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null,

            // Maximum Overkill (slots 80-99)
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

    }
}
