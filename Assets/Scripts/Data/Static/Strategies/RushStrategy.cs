using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Data.Static.Strategies
{
    public class RushStrategy : IBaseStrategy
    {
        public IList<IPrefabKeyWrapper> BuildOrder => StaticBuildOrders.Adaptive.Rush;
    }
}
