using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public interface ILootItem
    {
		// FELIX  Probably need to have method for adding newly unlocked item ti GameModel and player Loadout.
        // Can then probably remove this getter :)
        IPrefabKey ItemKey { get; }
        void ShowItemDetails(IPrefabFactory prefabFactory, IItemDetailsControllers itemDetailsControllers);
    }
}
