using BattleCruisers.AI.Providers;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public class AntiNavalPrefabKeyWrapper : BasePrefabKeyWrapper
    {
        protected override IDynamicBuildOrder GetBuildOrder(IBuildOrders buildOrders)
        {
            return buildOrders.AntiNavalBuildOrder;
        }
    }
}
