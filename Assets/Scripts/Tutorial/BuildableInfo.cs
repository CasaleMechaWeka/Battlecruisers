using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class BuildableInfo
    {
        public IPrefabKey Key { get; }
        // FELIX  Localise
        public string Name { get; }

        public BuildableInfo(IPrefabKey key, string name)
        {
            Assert.IsNotNull(key);

            Key = key;
            Name = name;
        }
    }
}
