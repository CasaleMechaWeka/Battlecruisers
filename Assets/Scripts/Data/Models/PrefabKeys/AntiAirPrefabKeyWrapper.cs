using System.Collections.Generic;
using BattleCruisers.AI.Providers;

namespace BattleCruisers.Data.Models.PrefabKeys
{
	public class AntiAirPrefabKeyWrapper : BasePrefabKeyWrapper
	{
		protected override IEnumerator<IPrefabKey> GetBuildOrder(IBuildOrders buildOrders)
		{
            return buildOrders.AntiAirBuildOrder;
		}
	}
}
