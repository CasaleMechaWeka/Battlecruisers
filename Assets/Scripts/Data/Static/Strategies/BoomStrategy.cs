using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Data.Static.Strategies
{
    public class BoomStrategy : IBaseStrategy
    {
        public IList<IPrefabKeyWrapper> BuildOrder => StaticBuildOrders.Adaptive.Boom;
    }
}
