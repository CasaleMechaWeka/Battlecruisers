using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
	public class LoadoutBuildingItem : LoadoutItem<Building>
	{
		public void Initialise(Building building, BuildingDetailsManager buildingDetailsManager)
		{
			InternalInitialise(building, buildingDetailsManager);
		}
	}
}
