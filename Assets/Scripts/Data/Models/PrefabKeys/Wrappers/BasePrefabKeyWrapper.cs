using System.Collections.Generic;
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
            IEnumerator<IPrefabKey> buildOrder = GetBuildOrder(buildOrders);

            HasKey = buildOrder.MoveNext();

			if (HasKey)
			{
				Key = buildOrder.Current;
				Assert.IsNotNull(Key);
			}
		}

        protected abstract IEnumerator<IPrefabKey> GetBuildOrder(IBuildOrders buildOrders);
	}
}
