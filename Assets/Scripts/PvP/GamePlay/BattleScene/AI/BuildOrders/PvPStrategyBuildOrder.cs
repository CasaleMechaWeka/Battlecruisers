using System.Collections.Generic;
using BattleCruisers.AI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public class PvPStrategyBuildOrder : IPvPDynamicBuildOrder
    {
        private readonly IEnumerator<IPvPPrefabKeyWrapper> _baseBuildOrder;
        private readonly IPvPLevelInfo _levelInfo;

        private PvPBuildingKey _current;
        public PvPBuildingKey Current
        {
            get { return _current; }
            private set
            {
                _current = value;
             //   Logging.Log(Tags.AI_BUILD_ORDERS, $"{this}.Current = {_current}");
            }
        }

        public PvPStrategyBuildOrder(IEnumerator<IPvPPrefabKeyWrapper> baseBuildOrder, IPvPLevelInfo levelInfo)
        {
            PvPHelper.AssertIsNotNull(baseBuildOrder, levelInfo);

            _baseBuildOrder = baseBuildOrder;
            _levelInfo = levelInfo;
        }

        public bool MoveNext()
        {
            while (_baseBuildOrder.MoveNext())
            {
/*                Logging.Log(
                    Tags.AI_BUILD_ORDERS,
                    $"{this}.MoveNext()  _baseBuildOrder.Current: {_baseBuildOrder.Current}  _baseBuildOrder.Current.HasKey: {_baseBuildOrder.Current.HasKey}");*/

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
