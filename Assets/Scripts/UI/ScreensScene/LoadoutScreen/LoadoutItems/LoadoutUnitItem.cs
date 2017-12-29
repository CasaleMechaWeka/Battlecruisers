using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
    public class LoadoutUnitItem : LoadoutItem<IUnit>
	{
        public void Initialise(IUnit building, UnitDetailsManager unitDetailsManager)
		{
			InternalInitialise(building, unitDetailsManager);
		}
	}
}
