using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Static
{
    public interface ILoot
    {
        int LevelNum { get; }
        IPrefabKey BuildingKey { get; }
        IPrefabKey UnitKey { get; }
        IPrefabKey HullKey { get; }
    }
}
