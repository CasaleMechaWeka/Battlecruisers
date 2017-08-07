using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.AI.Providers.Strategies
{
	public class BasicBoomAggressiveStrategy : IBaseStrategy
	{
        public IList<IPrefabKeyWrapper> BuildOrder { get { return StaticBuildOrders.Basic.BoomAggressive; } }
	}
}
