using BattleCruisers.Buildables.Units;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    // FELIX  Avoid duplicate code with BuildingDetailsManager?
    public class UnitDetailsManager : ItemDetailsManager<IUnit>
	{
        // FELIX  Initialise programmatically?
		public ComparableUnitDetailsController singleItemDetails, leftComparableItemDetails, rightComparableItemDetails;

		public void Initialise()
		{
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
			
			singleItemDetails.Initialise(spriteProvider);
			leftComparableItemDetails.Initialise(spriteProvider);
			rightComparableItemDetails.Initialise(spriteProvider);

			Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
		}
	}
}
