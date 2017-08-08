using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public class BuildOrders : IBuildOrders
	{
        public IEnumerator<IPrefabKey> OffensiveBuildOrder { get; private set; }
        public IEnumerator<IPrefabKey> AntiAirBuildOrder { get; private set; }
        public IEnumerator<IPrefabKey> AntiNavalBuildOrder { get; private set; }

        public BuildOrders(
            IList<IPrefabKey> offensiveBuildOrder,
	        IList<IPrefabKey> antiAirBuildOrder,
	        IList<IPrefabKey> antiNavalBuildOrder)
        {
            OffensiveBuildOrder = offensiveBuildOrder.GetEnumerator();
            AntiAirBuildOrder = antiAirBuildOrder.GetEnumerator();
            AntiNavalBuildOrder = antiNavalBuildOrder.GetEnumerator();
		}
	}
}
