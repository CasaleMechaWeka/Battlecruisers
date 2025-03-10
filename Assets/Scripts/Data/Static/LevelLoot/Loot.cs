using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public class Loot : ILoot
    {
        public ReadOnlyCollection<ILootItem> Items { get; }

        public Loot(
            IList<HullKey> hullKeys,
            IList<UnitKey> unitKeys,
            IList<BuildingKey> buildingKeys)
        {
            Helper.AssertIsNotNull(hullKeys, unitKeys, buildingKeys);

            IList<ILootItem> lootItems = new List<ILootItem>();

            foreach (HullKey hullKey in hullKeys)
            {
                lootItems.Add(new HullLootItem(hullKey));
            }

            foreach (UnitKey unitKey in unitKeys)
            {
                lootItems.Add(new UnitLootItem(unitKey));
            }

            foreach (BuildingKey buildingKey in buildingKeys)
            {
                lootItems.Add(new BuildingLootItem(buildingKey));
            }

            Items = new ReadOnlyCollection<ILootItem>(lootItems);
        }

        public override bool Equals(object obj)
        {
            Loot other = obj as Loot;

            return
                other != null
                && Enumerable.SequenceEqual(Items, other.Items);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Items);
        }
    }
}
