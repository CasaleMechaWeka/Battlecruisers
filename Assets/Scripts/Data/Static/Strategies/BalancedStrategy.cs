using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Data.Static.Strategies
{
    public class BalancedStrategy : IBaseStrategy
    {
        public IList<IPrefabKeyWrapper> BuildOrder => StaticBuildOrders.Adaptive.Balanced;
    }
}
