using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.BuildOrders
{
    public class InfiniteStaticBuildOrder : IDynamicBuildOrder
	{
        public BuildingKey Current { get; }

        public InfiniteStaticBuildOrder(BuildingKey buildingKey)
		{
			Current = buildingKey;
		}

        public bool MoveNext()
        {
            return true;
        }
    }
}