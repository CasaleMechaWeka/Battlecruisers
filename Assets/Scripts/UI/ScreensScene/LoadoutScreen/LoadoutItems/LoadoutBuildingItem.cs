using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
	public class LoadoutBuildingItem : LoadoutItem<IBuilding>
	{
        // FELIX  Perhaps use interface as parameter instead of BuildingDetailsManager?  (Also for IUnit counterpart)
		public void Initialise(IBuilding building, BuildingDetailsManager buildingDetailsManager)
		{
			InternalInitialise(building, buildingDetailsManager);
		}
	}
}
