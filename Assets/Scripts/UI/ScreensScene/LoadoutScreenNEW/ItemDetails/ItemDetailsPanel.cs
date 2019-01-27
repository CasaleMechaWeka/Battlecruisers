using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public class ItemDetailsPanel : MonoBehaviour, IItemDetailsPanel
    {
        private ComparableCruiserDetailsController _leftCruiserDetails, _rightCruiserDetails;
        public IComparableItemDetails<ICruiser> LeftCruiserDetails { get { return _leftCruiserDetails; } }
        public IComparableItemDetails<ICruiser> RightCruiserDetails { get { return _rightCruiserDetails; } }

        private ComparableBuildingDetailsController _leftBuildingDetails, _rightBuildingDetails;
        public IComparableItemDetails<IBuilding> LeftBuildingDetails { get { return _leftBuildingDetails; } }
        public IComparableItemDetails<IBuilding> RightBuildingDetails { get { return _rightBuildingDetails; } }

        private ComparableUnitDetailsController _leftUnitDetails, _rightUnitDetails;
        public IComparableItemDetails<IUnit> LeftUnitDetails { get { return _leftUnitDetails; } }
        public IComparableItemDetails<IUnit> RightUnitDetails { get { return _rightUnitDetails; } }

        private CompareButton _compareButton;

        public void FindComponents()
        {
            GetDetails();

            _compareButton = GetComponentInChildren<CompareButton>();
            Assert.IsNotNull(_compareButton);
        }

        private void GetDetails()
        {
            // Cruisers
            _leftCruiserDetails = transform.FindNamedComponent<ComparableCruiserDetailsController>("HullDetailsPanel/LeftDetails");
            _rightCruiserDetails = transform.FindNamedComponent<ComparableCruiserDetailsController>("HullDetailsPanel/RightDetails");

            // Buildings
            _leftBuildingDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("BuildingDetailsPanel/LeftDetails");
            _rightBuildingDetails = transform.FindNamedComponent<ComparableBuildingDetailsController>("BuildingDetailsPanel/RightDetails");

            // Units
            _leftUnitDetails = transform.FindNamedComponent<ComparableUnitDetailsController>("UnitDetailsPanel/LeftDetails");
            _rightUnitDetails = transform.FindNamedComponent<ComparableUnitDetailsController>("UnitDetailsPanel/RightDetails");
        }

        // Not part of GetComponents() due to circular dependency :)
        public void InitialiseComponents(IItemDetailsDisplayer itemDetailsDisplayer, IBroadcastingProperty<ItemFamily?> itemFamilyToCompare)
        {
            Helper.AssertIsNotNull(itemDetailsDisplayer, itemFamilyToCompare);

            InitialiseDetails();
            _compareButton.Initialise(itemDetailsDisplayer, itemFamilyToCompare);
        }

        private void InitialiseDetails()
        {
            _leftCruiserDetails.Initialise();
            _rightCruiserDetails.Initialise();
            _leftBuildingDetails.Initialise();
            _rightBuildingDetails.Initialise();
            _leftUnitDetails.Initialise();
            _rightUnitDetails.Initialise();
        }
    }
}