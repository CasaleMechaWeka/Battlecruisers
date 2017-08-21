using BattleCruisers.AI.Providers;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public class AntiAirPrefabKeyWrapper : BasePrefabKeyWrapper
	{
		protected override IDynamicBuildOrder GetBuildOrder(IBuildOrders buildOrders)
		{
            return buildOrders.AntiAirBuildOrder;
		}
	}
}
