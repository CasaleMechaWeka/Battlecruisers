using BattleCruisers.AI.BuildOrders;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public abstract class BasePrefabKeyWrapper : IPrefabKeyWrapper
	{
        private IDynamicBuildOrder _buildOrder;

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

                Logging.Log(Tags.AI_BUILD_ORDERS, $"HasKey: {_hasKey}  Key: {Key}");

                return _hasKey;
            }
        }
		
        public BuildingKey Key { get; private set; }

        protected BasePrefabKeyWrapper()
        {
            _haveAskedBuildOrder = false;
            _hasKey = false;
            Key = null;
        }

		public void Initialise(IBuildOrders buildOrders)
		{
            _buildOrder = GetBuildOrder(buildOrders);
		}

        protected abstract IDynamicBuildOrder GetBuildOrder(IBuildOrders buildOrders);
	}
}
