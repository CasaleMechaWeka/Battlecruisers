using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildOrders
    {
		IEnumerator<IPrefabKey> OffensiveBuildOrder { get; }

        // FELIX  Add:  Anti-Air & Anti-Naval
	}
}