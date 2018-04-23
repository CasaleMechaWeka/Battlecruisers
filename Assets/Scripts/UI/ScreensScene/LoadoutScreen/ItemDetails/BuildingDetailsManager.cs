using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class BuildingDetailsManager : ItemDetailsManager<IBuilding>
    {
        public void Initialise(ISpriteProvider spriteProvider, IItemStateManager itemStateManager)
        {
            ComparableBuildingDetailsController singleItemDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("SingleItemDetails");
            singleItemDetails.Initialise(spriteProvider);

            ComparableBuildingDetailsController leftComparableItemDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("LeftItemDetails");
            leftComparableItemDetails.Initialise(spriteProvider);

            ComparableBuildingDetailsController rightComparableItemDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("RightItemDetails");
            rightComparableItemDetails.Initialise(spriteProvider);

            Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails, itemStateManager);
        }
    }
}
