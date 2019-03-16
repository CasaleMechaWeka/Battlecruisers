using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Data.Static.Strategies
{
    public class BasicRushStrategy : IBaseStrategy
    {
        public IList<IPrefabKeyWrapper> BuildOrder => StaticBuildOrders.Basic.Rush;
    }
}
