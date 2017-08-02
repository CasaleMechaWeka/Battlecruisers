using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI
{
    public class BuildOrderProvider : IBuildOrderProvider
    {
		private const int LEVEL_SAM_SITE_IS_UNLOCKED = 2;
		private const int LEVEL_MORTAR_IS_UNLOCKED = 1;
		private const int LEVEL_TESLA_COIL_IS_UNLOCKED = 12;


        public IList<IPrefabKey> AntiRocketBuildOrder { get { return BuildOrders.AntiRocketLauncher; } }

        public IList<IPrefabKey> GetBasicBuildOrder(int levelNum)
        {
            // FELIX  Don't always return same build order :P
            return BuildOrders.Balanced;
        }
		
        public IList<IPrefabKey> GetAdvancedBuildOrder(int levelNum)
        {
            // FELIX  Don't always return same build order :P
            return BuildOrders.AdvancedBalanced;
        }

        public IList<IPrefabKey> GetAntiAirBuildOrder(int levelNum)
        {
            return levelNum > LEVEL_SAM_SITE_IS_UNLOCKED ? BuildOrders.AntiAir : BuildOrders.BasicAntiAir;
        }

        public IList<IPrefabKey> GetAntiNavalBuildOrder(int levelNum)
        {
            return levelNum > LEVEL_MORTAR_IS_UNLOCKED ? BuildOrders.AntiNaval : BuildOrders.BasicAntiNaval;
        }

        public bool IsAntiRocketBuildOrderAvailable(int levelNum)
        {
            return levelNum > LEVEL_TESLA_COIL_IS_UNLOCKED;
        }
    }
}
