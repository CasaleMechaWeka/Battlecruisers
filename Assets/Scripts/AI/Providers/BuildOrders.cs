using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
	public class BuildOrders : IBuildOrders
	{
        public IEnumerator<IPrefabKey> OffensiveBuildOrder { get; private set; }

        public BuildOrders(IList<IPrefabKey> offensiveBuildOrder)
        {
            OffensiveBuildOrder = offensiveBuildOrder.GetEnumerator();
        }
	}
}