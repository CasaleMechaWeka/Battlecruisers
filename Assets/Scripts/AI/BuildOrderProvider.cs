using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI
{
    public class BuildOrderProvider : IBuildOrderProvider
    {
		private const int LEVEL_SAM_SITE_IS_UNLOCKED = 2;
		private const int LEVEL_MORTAR_IS_UNLOCKED = 1;

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
    }
}
