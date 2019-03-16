using BattleCruisers.AI.BuildOrders;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public class StaticPrefabKeyWrapper : IPrefabKeyWrapper
	{
        public bool HasKey => true;
        public BuildingKey Key { get; }

        public StaticPrefabKeyWrapper(BuildingKey key)
        {
            Assert.IsNotNull(key);
            Key = key;
        }

        public void Initialise(IBuildOrders buildOrders) 
        { 
            // Do nothing
        }
	}
}
