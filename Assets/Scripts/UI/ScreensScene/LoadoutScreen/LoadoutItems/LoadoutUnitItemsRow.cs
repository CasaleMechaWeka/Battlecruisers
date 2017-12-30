using BattleCruisers.Buildables.Units;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
    public class LoadoutUnitItemsRow : LoadoutBuildableItemsRow<IUnit>
    {
        protected override LoadoutItem<IUnit> CreateItem(IUnit item)
        {
            return _uiFactory.CreateLoadoutUnitItem(layoutGroup, item);
        }
    }
}
