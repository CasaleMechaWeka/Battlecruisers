using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public interface ILoot
    {
        IPrefabKey BuildingKey { get; }
        IPrefabKey UnitKey { get; }
        IPrefabKey HullKey { get; }
    }
}
