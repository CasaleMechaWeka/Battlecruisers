using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class CruiserDetailsManager : ItemDetailsManager<ICruiser>
	{
        public void Initialise(IItemStateManager itemStateManager)
		{
            ComparableCruiserDetailsController singleItemDetails 
                = transform.FindNamedComponent<ComparableCruiserDetailsController>("SingleCruiserDetails");
            singleItemDetails.Initialise();

            ComparableCruiserDetailsController leftComparableItemDetails 
                = transform.FindNamedComponent<ComparableCruiserDetailsController>("LeftCruiserDetails");
            leftComparableItemDetails.Initialise();

            ComparableCruiserDetailsController rightComparableItemDetails 
                = transform.FindNamedComponent<ComparableCruiserDetailsController>("RightCruiserDetails");
            rightComparableItemDetails.Initialise();

            Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails, itemStateManager);
		}
	}
}
