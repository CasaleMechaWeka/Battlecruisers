using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class UnitDetailsManager : ItemDetailsManager<IUnit>
    {
        public void Initialise()
        {
            ComparableUnitDetailsController singleItemDetails = transform.FindNamedComponent<ComparableUnitDetailsController>("SingleItemDetails");
            singleItemDetails.Initialise();

            ComparableUnitDetailsController leftComparableItemDetails = transform.FindNamedComponent<ComparableUnitDetailsController>("LeftItemDetails");
            leftComparableItemDetails.Initialise();

            ComparableUnitDetailsController rightComparableItemDetails = transform.FindNamedComponent<ComparableUnitDetailsController>("RightItemDetails");
            rightComparableItemDetails.Initialise();

            Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails);
        }
    }
}
