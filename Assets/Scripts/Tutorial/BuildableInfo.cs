using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class BuildableInfo
    {
        public IPrefabKey Key { get; private set; }
        public string Name { get; private set; }

        public BuildableInfo(IPrefabKey key, string name)
        {
            Assert.IsNotNull(key);

            Key = key;
            Name = name;
        }
    }
}
