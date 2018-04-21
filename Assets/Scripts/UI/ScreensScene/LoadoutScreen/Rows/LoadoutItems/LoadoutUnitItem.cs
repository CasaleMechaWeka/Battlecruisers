using BattleCruisers.Buildables.Units;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutUnitItem : LoadoutItem<IUnit>
	{
        public override ItemType Type { get { return ItemType.Unit; } }
    }
}
