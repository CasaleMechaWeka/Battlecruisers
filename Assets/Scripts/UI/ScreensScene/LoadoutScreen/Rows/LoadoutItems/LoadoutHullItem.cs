using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutHullItem : LoadoutItem<ICruiser>
	{
		public void UpdateHull(ICruiser newHull)
		{
			Item = newHull;
		}
	}
}
