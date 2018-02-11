using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class CruiserDetailsManager : ItemDetailsManager<ICruiser>
	{
		public void Initialise()
		{
            CruiserDetailsController singleItemDetails = transform.FindNamedComponent<CruiserDetailsController>("SingleCruiserDetails");
            singleItemDetails.Initialise();

            CruiserDetailsController leftComparableItemDetails = transform.FindNamedComponent<CruiserDetailsController>("LeftCruiserDetails");
            leftComparableItemDetails.Initialise();

            CruiserDetailsController rightComparableItemDetails = transform.FindNamedComponent<CruiserDetailsController>("RightCruiserDetails");
            rightComparableItemDetails.Initialise();

			Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
		}
	}
}
