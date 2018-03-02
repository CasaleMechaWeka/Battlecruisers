using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;

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

        public override bool Equals(object obj)
        {
            Loot other = obj as Loot;

            return
                other != null
                && BuildingKey.SmartEquals(other.BuildingKey)
                && UnitKey.SmartEquals(other.UnitKey)
                && HullKey.SmartEquals(other.HullKey);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(BuildingKey, UnitKey, HullKey);
        }
    }
}
