using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Data.Static.LevelLoot
{
    public interface ILootItem
    {
		// FELIX  Probably need to have method for adding newly unlocked item to GameModel and player Loadout.

        void ShowItemDetails(IPrefabFactory prefabFactory, IItemDetailsGroup itemDetailsControllers);
    }
}
