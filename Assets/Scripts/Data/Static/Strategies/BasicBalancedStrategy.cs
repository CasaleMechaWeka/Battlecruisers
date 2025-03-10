using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Data.Static.Strategies
{
    public class BasicBalancedStrategy : IBaseStrategy
	{
        public IList<IPrefabKeyWrapper> BuildOrder => StaticBuildOrders.Basic.Balanced;
	}
}
