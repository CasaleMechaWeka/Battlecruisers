using System.Collections.ObjectModel;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public interface ILoot
    {
        ReadOnlyCollection<ILootItem> Items { get; }
    }
}
