using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class BuildingDetailsManager : ItemDetailsManager<IBuilding>
    {
        public void Initialise(IItemStateManager itemStateManager)
        {
            ComparableBuildingDetailsController singleItemDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("SingleItemDetails");
            singleItemDetails.Initialise();

            ComparableBuildingDetailsController leftComparableItemDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("LeftItemDetails");
            leftComparableItemDetails.Initialise();

            ComparableBuildingDetailsController rightComparableItemDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("RightItemDetails");
            rightComparableItemDetails.Initialise();

            Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails, itemStateManager);
        }
    }
}
