using BattleCruisers.AI.BuildOrders;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public class OffensivePrefabKeyWrapper : BasePrefabKeyWrapper
    {
        protected override IDynamicBuildOrder GetBuildOrder(IBuildOrders buildOrders)
        {
            return buildOrders.OffensiveBuildOrder;
        }
    }
}
