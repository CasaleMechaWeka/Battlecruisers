using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders
{
    public class PvPFiniteBuildOrder : IPvPDynamicBuildOrder
    {
        private readonly IPvPDynamicBuildOrder _infiniteBuildOrder;
        private readonly int _size;
        private int _index;

        public PvPBuildingKey Current { get; private set; }

        public PvPFiniteBuildOrder(IPvPDynamicBuildOrder infiniteBuildOrder, int size)
        {
            Assert.IsNotNull(infiniteBuildOrder);

            _infiniteBuildOrder = infiniteBuildOrder;
            _size = size;
            _index = 0;
        }

        public bool MoveNext()
        {
            bool hasKey = false;

            if (_index < _size)
            {
                hasKey = _infiniteBuildOrder.MoveNext();
                //Assert.IsTrue(hasKey, "Infinite build order should never run out of keys :/");

                Current = _infiniteBuildOrder.Current;
                _index++;
            }
            else
            {
                Current = null;
            }

            return hasKey;
        }
    }
}
