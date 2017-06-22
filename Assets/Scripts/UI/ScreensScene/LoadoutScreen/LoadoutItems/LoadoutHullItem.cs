using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
	public class LoadoutHullItem : LoadoutItem<Cruiser>
	{
		public void Initialise(Cruiser hull, CruiserDetailsManager cruiserDetailsManager)
		{
			InternalInitialise(hull, cruiserDetailsManager);
		}

		public void UpdateHull(Cruiser newHull)
		{
			Item = newHull;
		}
	}
}
