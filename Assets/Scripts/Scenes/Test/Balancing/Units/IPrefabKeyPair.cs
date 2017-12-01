using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public interface IPrefabKeyPair
    {
        IPrefabKey LeftKey { get; }
        IPrefabKey RightKey { get; }
    }
}
