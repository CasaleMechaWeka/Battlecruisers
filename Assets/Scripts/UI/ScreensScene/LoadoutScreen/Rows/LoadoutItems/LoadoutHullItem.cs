using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutHullItem : LoadoutItem<ICruiser>
	{
        public override ItemType Type { get { return ItemType.Cruiser; } }

		public void UpdateHull(ICruiser newHull)
		{
			_item = newHull;
		}
	}
}
