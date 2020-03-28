using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Data.Static.Strategies
{
    public class BasicBoomDefensiveStrategy : IBaseStrategy
	{
        public IList<IPrefabKeyWrapper> BuildOrder => StaticBuildOrders.Basic.BoomDefensive;
	}
}
