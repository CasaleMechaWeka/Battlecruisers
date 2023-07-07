using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers
{
    public abstract class PvPBasePrefabKeyWrapper : IPvPPrefabKeyWrapper
    {
        private IPvPDynamicBuildOrder _buildOrder;

        private bool _haveAskedBuildOrder;
        private bool _hasKey;
        public bool HasKey
        {
            get
            {
                if (!_haveAskedBuildOrder)
                {
                    // Evaluate lazily, to take advantage of dynamic build order :)
                    _haveAskedBuildOrder = true;

                    Assert.IsNotNull(_buildOrder, "Should call Initialise() before accessing this property :/");
                    _hasKey = _buildOrder.MoveNext();

                    if (_hasKey)
                    {
                        Key = _buildOrder.Current;
                        Assert.IsNotNull(Key);
                    }
                }

                //  Logging.Log(Tags.AI_BUILD_ORDERS, $"HasKey: {_hasKey}  Key: {Key}");

                return _hasKey;
            }
        }

        public PvPBuildingKey Key { get; private set; }

        protected PvPBasePrefabKeyWrapper()
        {
            _haveAskedBuildOrder = false;
            _hasKey = false;
            Key = null;
        }

        public void Initialise(IPvPBuildOrders buildOrders)
        {
            _buildOrder = GetBuildOrder(buildOrders);
        }

        protected abstract IPvPDynamicBuildOrder GetBuildOrder(IPvPBuildOrders buildOrders);
    }
}
