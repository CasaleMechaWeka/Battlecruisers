using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Data.Static.Strategies
{
	public class BasicTurtleStrategy : IBaseStrategy
	{
        public IList<IPrefabKeyWrapper> BuildOrder { get { return StaticBuildOrders.Basic.Turtle; } }
	}
}
