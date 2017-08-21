using BattleCruisers.AI.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public abstract class BasePrefabKeyWrapper : IPrefabKeyWrapper
	{
		public bool HasKey { get; private set; }
		public IPrefabKey Key { get; private set; }

		public void Initialise(IBuildOrders buildOrders)
		{
            IDynamicBuildOrder buildOrder = GetBuildOrder(buildOrders);

            HasKey = buildOrder.MoveNext();

			if (HasKey)
			{
				Key = buildOrder.Current;
				Assert.IsNotNull(Key);
			}
		}

        protected abstract IDynamicBuildOrder GetBuildOrder(IBuildOrders buildOrders);
	}
}
