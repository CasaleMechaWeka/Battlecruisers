using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class PrefabKeyPair : IPrefabKeyPair
    {
        public IPrefabKey LeftKey { get; private set; }
        public IPrefabKey RightKey { get; private set; }

        public PrefabKeyPair(IPrefabKey leftKey, IPrefabKey rightKey)
        {
            LeftKey = leftKey;
            RightKey = rightKey;
        }
    }
}
