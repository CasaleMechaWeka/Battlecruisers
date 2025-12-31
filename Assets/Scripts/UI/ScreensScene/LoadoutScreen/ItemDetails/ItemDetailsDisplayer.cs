using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class ItemDetailsDisplayer<TItem> where TItem : class, IComparableItem
    {
        private readonly IComparableItemDetails<TItem> _leftDetails, _rightDetails;
        private ISettableBroadcastingProperty<TItem> _selectedItem;
        public IBroadcastingProperty<TItem> SelectedItem { get; }

        public ItemDetailsDisplayer(IComparableItemDetails<TItem> leftDetails, IComparableItemDetails<TItem> rightDetails)
        {
            Helper.AssertIsNotNull(leftDetails, rightDetails);
            _leftDetails = leftDetails;
            _rightDetails = rightDetails;
            _selectedItem = new SettableBroadcastingProperty<TItem>(initialValue: null);
            SelectedItem = new BroadcastingProperty<TItem>(_selectedItem);
        }

        public void SelectItem(TItem item)
        {
            Assert.IsNotNull(item);
            HideDetails();
            _selectedItem.Value = item;
            _leftDetails.ShowItemDetails(item);
        }

        public void SelectItem(HullType hullType)
        {
            _leftDetails.SetHullType(hullType);
        }
        public void SelectItem(IBuilding building)
        {
            _leftDetails.SetBuilding(building);
        }
        public void SelectItem(IBuilding building, ItemButton button)
        {
            _leftDetails.SetBuilding(building, button);
        }
        public void SelectItem(IUnit unit)
        {
            _leftDetails.SetUnit(unit);
        }
        public void SelectItem(IUnit unit, ItemButton button)
        {
            _leftDetails.SetUnit(unit, button);
        }

        public void CompareWithSelectedItem(TItem item)
        {
            Assert.IsNotNull(item);
            Assert.IsNotNull(_selectedItem.Value);
            _leftDetails.ShowItemDetails(_selectedItem.Value, item);
            _rightDetails.ShowItemDetails(item, _selectedItem.Value);
        }

        public void HideDetails()
        {
            _leftDetails.Hide();
            _rightDetails.Hide();
            _selectedItem.Value = null;
        }
    }
}