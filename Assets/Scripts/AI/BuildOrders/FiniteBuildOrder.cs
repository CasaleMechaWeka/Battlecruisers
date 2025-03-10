using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.BuildOrders
{
    public class FiniteBuildOrder : IDynamicBuildOrder
	{
        private readonly IDynamicBuildOrder _infiniteBuildOrder;
        private readonly int _size;
        private int _index;

        public BuildingKey Current { get; private set; }

		public FiniteBuildOrder(IDynamicBuildOrder infiniteBuildOrder, int size)
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
