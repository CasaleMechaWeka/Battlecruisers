using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class ItemDetailsPanel : MonoBehaviour, IItemDetailsPanel
    {
        public ComparableCruiserDetailsController leftCruiserDetails, rightCruiserDetails;
        public IComparableItemDetails<ICruiser> LeftCruiserDetails => leftCruiserDetails;
        public IComparableItemDetails<ICruiser> RightCruiserDetails => rightCruiserDetails;

        public ComparableBuildingDetailsController leftBuildingDetails, rightBuildingDetails;
        public IComparableItemDetails<IBuilding> LeftBuildingDetails => leftBuildingDetails;
        public IComparableItemDetails<IBuilding> RightBuildingDetails => rightBuildingDetails;

        public ComparableUnitDetailsController leftUnitDetails, rightUnitDetails;
        public IComparableItemDetails<IUnit> LeftUnitDetails => leftUnitDetails;
        public IComparableItemDetails<IUnit> RightUnitDetails => rightUnitDetails;

        public void Initialise()
        {
            Helper.AssertIsNotNull(leftCruiserDetails, rightCruiserDetails, leftBuildingDetails, rightBuildingDetails, leftUnitDetails, rightUnitDetails);

            leftCruiserDetails.Initialise();
            rightCruiserDetails.Initialise();

            leftBuildingDetails.Initialise();
            rightBuildingDetails.Initialise();

            leftUnitDetails.Initialise();
            rightUnitDetails.Initialise();
        }
    }
}