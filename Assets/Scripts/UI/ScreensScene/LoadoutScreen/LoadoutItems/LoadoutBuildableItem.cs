using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
	public class LoadoutBuildableItem : LoadoutItem<Buildable>
	{
		public void Initialise(Buildable buildable, BuildableDetailsManager buildableDetailsManager)
		{
			InternalInitialise(buildable, buildableDetailsManager);
		}
	}
}
