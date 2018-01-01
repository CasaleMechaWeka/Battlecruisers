using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutBuildingItemsRow : LoadoutBuildableItemsRow<IBuilding>
    {
        protected override LoadoutItem<IBuilding> CreateItem(IBuilding item)
        {
            return _uiFactory.CreateLoadoutBuildingItem(layoutGroup, item);
        }
    }
}
