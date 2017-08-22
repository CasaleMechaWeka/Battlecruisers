using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public class InfiniteStaticBuildOrder : IDynamicBuildOrder
	{
		public IPrefabKey Current { get; private set; }

        public InfiniteStaticBuildOrder(IPrefabKey buildingKey)
		{
			Current = buildingKey;
		}

        public bool MoveNext()
        {
            return true;
        }
    }
}