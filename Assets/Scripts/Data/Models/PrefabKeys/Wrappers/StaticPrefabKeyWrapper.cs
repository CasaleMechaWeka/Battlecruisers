using BattleCruisers.AI.Providers;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Models.PrefabKeys
{
    public class StaticPrefabKeyWrapper : IPrefabKeyWrapper
	{
        public bool HasKey { get { return true; } }
        public IPrefabKey Key { get; private set; }

        public StaticPrefabKeyWrapper(IPrefabKey key)
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
