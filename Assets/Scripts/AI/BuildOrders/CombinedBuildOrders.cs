using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.BuildOrders
{
    public class CombinedBuildOrders : IDynamicBuildOrder
	{
        private readonly IList<IDynamicBuildOrder> _buildOrders;

        public BuildingKey Current { get; private set; }

        public CombinedBuildOrders(IList<IDynamicBuildOrder> buildOrders)
		{
            Assert.IsNotNull(buildOrders);
            Assert.IsTrue(buildOrders.Count > 0);

            _buildOrders = buildOrders;
		}

		public bool MoveNext()
		{
            IDynamicBuildOrder validBuildOrder = _buildOrders.FirstOrDefault(buildOrder => buildOrder.MoveNext());
            Current = validBuildOrder?.Current;
            return Current != null;
		}
	}
}
