using BattleCruisers.AI.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
	public class OffensivePrefabKeyWrapper : IPrefabKeyWrapper
	{
		public bool HasKey { get; private set; }
		public IPrefabKey Key { get; private set; }

		public void Initialise(IBuildOrders buildOrders)
		{
            HasKey = buildOrders.OffensiveBuildOrder.MoveNext();

            if (HasKey)
            {
                Key = buildOrders.OffensiveBuildOrder.Current;
                Assert.IsNotNull(Key);
            }
		}
	}
}
