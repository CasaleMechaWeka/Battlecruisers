using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class UnitDetailsManager : ItemDetailsManager<IUnit>
    {
        public void Initialise(IItemStateManager itemStateManager)
        {
            ComparableUnitDetailsController singleItemDetails 
                = transform.FindNamedComponent<ComparableUnitDetailsController>("Single/UnitDetails");
            singleItemDetails.Initialise();

            ComparableUnitDetailsController leftComparableItemDetails 
                = transform.FindNamedComponent<ComparableUnitDetailsController>("Left/UnitDetails");
            leftComparableItemDetails.Initialise();

            ComparableUnitDetailsController rightComparableItemDetails 
                = transform.FindNamedComponent<ComparableUnitDetailsController>("Right/UnitDetails");
            rightComparableItemDetails.Initialise();

            Initialise(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails,itemStateManager);
        }
    }
}
