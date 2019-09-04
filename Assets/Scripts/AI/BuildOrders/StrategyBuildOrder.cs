using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.BuildOrders
{
    public class StrategyBuildOrder : IDynamicBuildOrder
    {
        private readonly IEnumerator<IPrefabKeyWrapper> _baseBuildOrder;
        private readonly ILevelInfo _levelInfo;

        private BuildingKey _current;
        public BuildingKey Current
        {
            get { return _current; }
            private set
            {
                _current = value;
                Logging.Log(Tags.AI_BUILD_ORDERS, $"{this}.Current = {_current}");
            }
        }

        public StrategyBuildOrder(IEnumerator<IPrefabKeyWrapper> baseBuildOrder, ILevelInfo levelInfo)
        {
            Helper.AssertIsNotNull(baseBuildOrder, levelInfo);

            _baseBuildOrder = baseBuildOrder;
            _levelInfo = levelInfo;
        }

        public bool MoveNext()
        {
            while (_baseBuildOrder.MoveNext())
            {
                Logging.Log(
                    Tags.AI_BUILD_ORDERS, 
                    $"{this}.MoveNext()  _baseBuildOrder.Current: {_baseBuildOrder.Current}  _baseBuildOrder.Current.HasKey: {_baseBuildOrder.Current.HasKey}");

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
