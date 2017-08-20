using System.Linq;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers.BuildingKey
{
    public class CombinedBuildOrders : IDynamicBuildOrder
	{
        private readonly IDynamicBuildOrder[] _buildOrders;

		public IPrefabKey Current { get; private set; }

        public CombinedBuildOrders(params IDynamicBuildOrder[] buildOrders)
		{
            Assert.IsNotNull(buildOrders);
            Assert.IsTrue(buildOrders.Length > 0);

            _buildOrders = buildOrders;
		}

		public bool MoveNext()
		{
            IDynamicBuildOrder validBuildOrder = _buildOrders.FirstOrDefault(buildOrder => buildOrder.MoveNext());
            Current = validBuildOrder != null ? validBuildOrder.Current : null;
            return Current != null;
		}
	}
}
