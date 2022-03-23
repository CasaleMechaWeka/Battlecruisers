using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Data.Static.Strategies
{
    public class BasicBOOMStrategy : IBaseStrategy
	{
        public IList<IPrefabKeyWrapper> BuildOrder => StaticBuildOrders.Basic.BOOM;
	}
}
