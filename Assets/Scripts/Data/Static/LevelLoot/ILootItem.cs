using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public interface ILootItem
    {
        IPrefabKey ItemKey { get; }
        void ShowItemDetails(IPrefabFactory prefabFactory, IItemDetailsControllers itemDetailsControllers);
    }
}
