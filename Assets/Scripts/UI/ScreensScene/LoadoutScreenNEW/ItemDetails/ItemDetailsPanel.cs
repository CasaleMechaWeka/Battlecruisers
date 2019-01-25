using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public class ItemDetailsPanel : MonoBehaviour, IItemDetailsPanel
    {
        public IComparableItemDetails<ICruiser> LeftCruiserDetails { get; private set; }
        public IComparableItemDetails<ICruiser> RightCruiserDetails { get; private set; }

        public IComparableItemDetails<IBuilding> LeftBuildingDetails { get; private set; }
        public IComparableItemDetails<IBuilding> RightBuildingDetails { get; private set; }

        public IComparableItemDetails<IUnit> LeftUnitDetails { get; private set; }
        public IComparableItemDetails<IUnit> RightUnitDetails { get; private set; }

        public void Initialise()
        {
            ComparableBuildingDetailsController leftBuildingDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("BuildingDetailsPanel/LeftDetails");
            leftBuildingDetails.Initialise();
            LeftBuildingDetails = leftBuildingDetails;

            ComparableBuildingDetailsController rightBuildingDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("BuildingDetailsPanel/RightDetails");
            rightBuildingDetails.Initialise();
            RightBuildingDetails = rightBuildingDetails;

            // FELIX  Initialise others :)
        }
    }
}