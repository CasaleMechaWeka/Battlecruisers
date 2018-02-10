using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class CruiserDetailsManager : ItemDetailsManager<ICruiser>
	{
		public CruiserDetailsController singleItemDetails, leftComparableItemDetails, rightComparableItemDetails;

		public void Initialise()
		{
            // FELIX  Assign programmatically like in BuidlableDetailsManager
            singleItemDetails.Initialise();
            leftComparableItemDetails.Initialise();
            rightComparableItemDetails.Initialise();

			Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
		}
	}
}
