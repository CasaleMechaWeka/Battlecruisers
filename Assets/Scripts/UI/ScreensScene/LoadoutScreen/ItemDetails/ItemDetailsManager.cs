using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class ItemDetailsManager
    {
        private readonly ItemDetailsDisplayer<IBuilding> _buildingDetails;
        private readonly ItemDetailsDisplayer<IUnit> _unitDetails;
        private readonly ItemDetailsDisplayer<ICruiser> _cruiserDetails;

        public ProfileDetailsController ProfileDetails;

        public ItemFamily? SelectedItemFamily { get; private set; }

        private readonly ISettableBroadcastingProperty<int> _numOfDetailsShown;
        public IBroadcastingProperty<int> NumOfDetailsShown { get; }

        private readonly ISettableBroadcastingProperty<IComparableItem> _selectedItem;
        public IBroadcastingProperty<IComparableItem> SelectedItem { get; }

        private readonly ISettableBroadcastingProperty<IComparableItem> _comparingItem;
        public IBroadcastingProperty<IComparableItem> ComparingItem { get; }

        public ItemDetailsManager(
            ItemDetailsDisplayer<IBuilding> buildingDetails,
            ItemDetailsDisplayer<IUnit> unitDetails,
            ItemDetailsDisplayer<ICruiser> cruiserDetails)
        {
            Helper.AssertIsNotNull(buildingDetails, unitDetails, cruiserDetails);

            _buildingDetails = buildingDetails;
            _unitDetails = unitDetails;
            _cruiserDetails = cruiserDetails;

            SelectedItemFamily = null;

            _numOfDetailsShown = new SettableBroadcastingProperty<int>(initialValue: 0);
            NumOfDetailsShown = new BroadcastingProperty<int>(_numOfDetailsShown);

            _selectedItem = new SettableBroadcastingProperty<IComparableItem>(initialValue: null);
            SelectedItem = new BroadcastingProperty<IComparableItem>(_selectedItem);

            _comparingItem = new SettableBroadcastingProperty<IComparableItem>(initialValue: null);
            ComparingItem = new BroadcastingProperty<IComparableItem>(_comparingItem);
        }

        public void ShowDetails(IBuilding building)
        {
            ShowDetails(building, _buildingDetails, ItemFamily.Buildings);
            _buildingDetails.SelectItem(building);
        }

        public void ShowDetails(IBuilding building, ItemButton button)
        {
            ShowDetails(building, _buildingDetails, ItemFamily.Buildings);
            //    _buildingDetails.SelectItem(building);
            _buildingDetails.SelectItem(building, button);
        }

        public void ShowDetails(IUnit unit)
        {
            ShowDetails(unit, _unitDetails, ItemFamily.Units);
            _unitDetails.SelectItem(unit);
        }

        public void ShowDetails(IUnit unit, ItemButton button)
        {
            ShowDetails(unit, _unitDetails, ItemFamily.Units);
            _unitDetails.SelectItem(unit, button);
        }

        public void ShowDetails(ICruiser cruiser)
        {
            ShowDetails(cruiser, _cruiserDetails, ItemFamily.Hulls);
        }

        public void ShowDetails(HullType hullType)
        {
            ShowDetails(_cruiserDetails, hullType);
        }

        public void ShowProfile()
        {
            HideDetails();
            SelectedItemFamily = ItemFamily.Profile;
            ProfileDetails.ShowProfile();
            //  itemDetails.SelectItem(item);
            //  _selectedItem.Value = item;
            _numOfDetailsShown.Value = 1;
        }

        private void ShowDetails(ItemDetailsDisplayer<ICruiser> itemDetails, HullType hullType)
        {
            itemDetails.SelectItem(hullType);
        }

        private void ShowDetails<TItem, TItemDetails>(TItem item, TItemDetails itemDetails, ItemFamily itemFamily)
            where TItem : class, IComparableItem
            where TItemDetails : ItemDetailsDisplayer<TItem>
        {
            Assert.IsNotNull(item);

            HideDetails();
            SelectedItemFamily = itemFamily;
            itemDetails.SelectItem(item);
            _selectedItem.Value = item;
            _numOfDetailsShown.Value = 1;
        }

        public void CompareWithSelectedItem(IBuilding building)
        {
            CompareWithSelectedItem(building, _buildingDetails, ItemFamily.Buildings);
        }

        public void CompareWithSelectedItem(IUnit unit)
        {
            CompareWithSelectedItem(unit, _unitDetails, ItemFamily.Units);
        }

        public void CompareWithSelectedItem(ICruiser cruiser)
        {
            CompareWithSelectedItem(cruiser, _cruiserDetails, ItemFamily.Hulls);
        }

        private void CompareWithSelectedItem<TItem, TItemDetails>(TItem item, TItemDetails itemDetails, ItemFamily itemFamily)
            where TItem : class, IComparableItem
            where TItemDetails : ItemDetailsDisplayer<TItem>
        {
            Assert.IsNotNull(item);
            Assert.AreEqual(SelectedItemFamily, itemFamily);

            itemDetails.CompareWithSelectedItem(item);
            _comparingItem.Value = item;
            _numOfDetailsShown.Value = 2;
        }

        public void HideDetails()
        {
            _buildingDetails.HideDetails();
            _unitDetails.HideDetails();
            _cruiserDetails.HideDetails();
            ProfileDetails.HideDetails();
            _selectedItem.Value = null;
            _comparingItem.Value = null;
            _numOfDetailsShown.Value = 0;
        }
    }
}