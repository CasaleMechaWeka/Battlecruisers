using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildOrders
    {
		IEnumerator<IPrefabKey> OffensiveBuildOrder { get; }
		IEnumerator<IPrefabKey> AntiAirBuildOrder { get; }
		IEnumerator<IPrefabKey> AntiNavalBuildOrder { get; }
	}
}