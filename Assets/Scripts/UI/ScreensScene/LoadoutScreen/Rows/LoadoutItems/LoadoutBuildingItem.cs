using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutBuildingItem : LoadoutItem<IBuilding>
	{
        public override ItemType Type { get { return ItemType.Building; } }
    }
}
