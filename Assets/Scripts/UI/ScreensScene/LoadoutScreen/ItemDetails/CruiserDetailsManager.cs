using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class CruiserDetailsManager : ItemDetailsManager<ICruiser>
	{
		public void Initialise()
		{
            CruiserDetailsController singleItemDetails = transform.FindNamedComponent<CruiserDetailsController>("SingleItemDetails");
            singleItemDetails.Initialise();

            CruiserDetailsController leftComparableItemDetails = transform.FindNamedComponent<CruiserDetailsController>("LeftItemDetails");
            leftComparableItemDetails.Initialise();

            CruiserDetailsController rightComparableItemDetails = transform.FindNamedComponent<CruiserDetailsController>("RightItemDetails");
            rightComparableItemDetails.Initialise();

			Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
		}
	}
}
