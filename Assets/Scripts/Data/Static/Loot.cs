using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Static
{
    public class Loot : ILoot
    {
        public IPrefabKey BuildingKey { get; private set; }
        public IPrefabKey UnitKey { get; private set; }
        public IPrefabKey HullKey { get; private set; }

        public Loot(
            IPrefabKey buildingKey = null,
            IPrefabKey unitKey = null,
            IPrefabKey hullKey = null)
        {
            BuildingKey = buildingKey;
            UnitKey = unitKey;
            HullKey = hullKey;
        }
    }
}
