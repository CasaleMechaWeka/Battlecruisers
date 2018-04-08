using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.BuildOrders
{
    // FELIX  Create test!
    public class StrategyBuildOrder : IDynamicBuildOrder
    {
        private readonly IEnumerator<IPrefabKeyWrapper> _baseBuildOrder;
        private readonly ILevelInfo _levelInfo;

        public BuildingKey Current { get; private set; }

        public StrategyBuildOrder(IList<IPrefabKeyWrapper> baseBuildOrder, ILevelInfo levelInfo)
        {
            Helper.AssertIsNotNull(baseBuildOrder, levelInfo);

            _baseBuildOrder = baseBuildOrder.GetEnumerator();
            _levelInfo = levelInfo;
        }

        public bool MoveNext()
        {
            while (_baseBuildOrder.MoveNext())
            {
                if (_baseBuildOrder.Current.HasKey 
                    && _levelInfo.CanConstructBuilding(_baseBuildOrder.Current.Key))
                {
                    Current = _baseBuildOrder.Current.Key;
                    return true;
                }
            }

            return false;
        }
    }
}
