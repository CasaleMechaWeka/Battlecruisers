using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Static
{
    public class Loot : ILoot
    {
        public int LevelNum { get; private set; }
        public IPrefabKey BuildingKey { get; private set; }
        public IPrefabKey UnitKey { get; private set; }
        public IPrefabKey HullKey { get; private set; }

        public Loot(
            int levelNum, 
            IPrefabKey buildingKey = null,
            IPrefabKey unitKey = null,
            IPrefabKey hullKey = null)
        {
            LevelNum = levelNum;
            BuildingKey = buildingKey;
            UnitKey = unitKey;
            HullKey = hullKey;
        }
    }
}
