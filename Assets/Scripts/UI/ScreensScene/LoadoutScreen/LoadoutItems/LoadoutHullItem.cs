using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
    public class LoadoutHullItem : LoadoutItem<ICruiser>
	{
		public void UpdateHull(ICruiser newHull)
		{
			Item = newHull;
		}
	}
}
