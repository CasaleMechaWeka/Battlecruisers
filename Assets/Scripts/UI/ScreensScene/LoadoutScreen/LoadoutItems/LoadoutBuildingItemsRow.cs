using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
    public class LoadoutBuildingItemsRow : LoadoutBuildableItemsRow<IBuilding>
    {
        protected override LoadoutItem<IBuilding> CreateItem(IBuilding item)
        {
            return _uiFactory.CreateLoadoutItem(layoutGroup, item);
        }
    }
}
