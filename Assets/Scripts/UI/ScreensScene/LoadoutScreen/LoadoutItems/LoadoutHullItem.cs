using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
	public class LoadoutHullItem : LoadoutItem<ICruiser>
	{
		public void Initialise(ICruiser hull, CruiserDetailsManager cruiserDetailsManager)
		{
			InternalInitialise(hull, cruiserDetailsManager);
		}

		public void UpdateHull(ICruiser newHull)
		{
			Item = newHull;
		}
	}
}
