using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.AI.Providers
{
    public class BuildOrderProvider : IBuildOrderProvider
    {
        // FELIX  Use StaticData instead
		private const int LEVEL_SAM_SITE_IS_UNLOCKED = 2;
		private const int LEVEL_MORTAR_IS_UNLOCKED = 1;
		private const int LEVEL_TESLA_COIL_IS_UNLOCKED = 12;

        public IList<IPrefabKey> AntiRocketBuildOrder { get { return StaticBuildOrders.AntiRocketLauncher; } }

        // FELIX  Avoid duplicate code?
        /// <summary>
        /// Gets the basic build order, which contains threat counters.
        /// </summary>
        public IList<IPrefabKey> GetBasicBuildOrder(int levelNum)
        {
			// Create offensive build order

			// Create base build order

			// FELIX  Don't always return same build order :P
			return StaticBuildOrders.Balanced;
        }
		
        /// <summary>
        /// Build orders do NOT contain threat counters.  These counters
        /// get created on the fly in response to threats.
        /// </summary>
        public IList<IPrefabKey> GetAdaptiveBuildOrder(int levelNum)
        {
            // Get base strategy for level (balanced, boom, rush)

            // Get offensive strategy for level (naval, air, offensive buildings, ultras)

            // Create offensive build order

            // Create BuildOrders class

            // Initialise build order



            // FELIX
            // FELIX  Don't always return same build order :P
            return new List<IPrefabKey>();
            //return StaticBuildOrders.AdaptiveBalancedBase;
        }

        public IList<IPrefabKey> GetAntiAirBuildOrder(int levelNum)
        {
            return levelNum > LEVEL_SAM_SITE_IS_UNLOCKED ? StaticBuildOrders.AntiAir : StaticBuildOrders.BasicAntiAir;
        }

        public IList<IPrefabKey> GetAntiNavalBuildOrder(int levelNum)
        {
            return levelNum > LEVEL_MORTAR_IS_UNLOCKED ? StaticBuildOrders.AntiNaval : StaticBuildOrders.BasicAntiNaval;
        }

        public bool IsAntiRocketBuildOrderAvailable(int levelNum)
        {
            return levelNum > LEVEL_TESLA_COIL_IS_UNLOCKED;
        }
    }
}
