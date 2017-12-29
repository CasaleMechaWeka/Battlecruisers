using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
    public class LoadoutUnitItem : LoadoutItem<IUnit>
	{
        public void Initialise(IUnit building, IItemDetailsManager<IUnit> unitDetailsManager)
		{
			InternalInitialise(building, unitDetailsManager);
		}
	}
}
