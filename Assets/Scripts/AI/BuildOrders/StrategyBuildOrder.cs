using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.BuildOrders
{
    public class StrategyBuildOrder : IDynamicBuildOrder
    {
        private readonly IEnumerator<IPrefabKeyWrapper> _baseBuildOrder;

        public IPrefabKey Current { get; private set; }

        public StrategyBuildOrder(IList<IPrefabKeyWrapper> baseBuildOrder)
        {
            Assert.IsNotNull(baseBuildOrder);
            _baseBuildOrder = baseBuildOrder.GetEnumerator();
        }

        public bool MoveNext()
        {
            while (_baseBuildOrder.MoveNext())
            {
                if (_baseBuildOrder.Current.HasKey)
                {
                    Current = _baseBuildOrder.Current.Key;
                    return true;
                }
            }

            return false;
        }
    }
}
