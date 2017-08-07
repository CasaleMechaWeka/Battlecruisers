using System.Collections.Generic;
using System.Linq;
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
	        IList<IPrefabKey> antiAirBuildOrder = null,
	        IList<IPrefabKey> antiNavalBuildOrder = null)
        {
            OffensiveBuildOrder = offensiveBuildOrder.GetEnumerator();

            AntiAirBuildOrder = antiAirBuildOrder != null ? 
                antiAirBuildOrder.GetEnumerator() : 
                Enumerable.Empty<IPrefabKey>().GetEnumerator();

            AntiNavalBuildOrder = antiNavalBuildOrder != null ?
                antiNavalBuildOrder.GetEnumerator() :
				Enumerable.Empty<IPrefabKey>().GetEnumerator();
		}
	}
}
