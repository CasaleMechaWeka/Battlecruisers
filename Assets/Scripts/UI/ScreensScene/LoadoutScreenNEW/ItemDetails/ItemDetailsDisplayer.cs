using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    // FELIX  Test :)
    public class ItemDetailsDisplayer : IItemDetailsDisplayer
    {
        private readonly IItemFamilyDetailsDisplayer<IBuilding> _buildingDetails;
        private readonly IItemFamilyDetailsDisplayer<IUnit> _unitDetails;
        private readonly IItemFamilyDetailsDisplayer<ICruiser> _cruiserDetails;

        public ItemFamily? SelectedItemFamily { get; private set; }

        public ItemDetailsDisplayer(
            IItemFamilyDetailsDisplayer<IBuilding> buildingDetails,
            IItemFamilyDetailsDisplayer<IUnit> unitDetails,
            IItemFamilyDetailsDisplayer<ICruiser> cruiserDetails)
        {
            Helper.AssertIsNotNull(buildingDetails, unitDetails, cruiserDetails);

            _buildingDetails = buildingDetails;
            _unitDetails = unitDetails;
            _cruiserDetails = cruiserDetails;

            SelectedItemFamily = null;
        }

        public void ShowDetails(IBuilding building)
        {
            ShowDetails(building, _buildingDetails, ItemFamily.Buildings);
        }

        public void ShowDetails(IUnit unit)
        {
            ShowDetails(unit, _unitDetails, ItemFamily.Units);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            ShowDetails(cruiser, _cruiserDetails, ItemFamily.Hulls);
        }

        private void ShowDetails<TItem, TItemDetails>(TItem item, TItemDetails itemDetails, ItemFamily itemFamily)
            where TItem : class, IComparableItem
            where TItemDetails : IItemFamilyDetailsDisplayer<TItem>
        {
            Assert.IsNotNull(item);

            HideDetails();
            SelectedItemFamily = itemFamily;
            itemDetails.SelectItem(item);
        }

        public void CompareWithSelectedItem(IBuilding building)
        {
            CompareWithSelectedItem(building, _buildingDetails);
        }

        public void CompareWithSelectedItem(IUnit unit)
        {
            CompareWithSelectedItem(unit, _unitDetails);
        }

        public void CompareWithSelectedItem(ICruiser cruiser)
        {
            CompareWithSelectedItem(cruiser, _cruiserDetails);
        }

        private void CompareWithSelectedItem<TItem, TItemDetails>(TItem item, TItemDetails itemDetails)
            where TItem : class, IComparableItem
            where TItemDetails : IItemFamilyDetailsDisplayer<TItem>
        {
            Assert.IsNotNull(item);
            Assert.AreEqual(ItemFamily.Buildings, SelectedItemFamily);

            itemDetails.CompareWithSelectedItem(item);
        }

        public void HideDetails()
        {
            _buildingDetails.HideDetails();
            _unitDetails.HideDetails();
            _cruiserDetails.HideDetails();
        }
    }
}