using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public class ItemDetailsDisplayer<TItem> : IItemDetailsDisplayer<TItem> where TItem : class, IComparableItem
    {
        private readonly IComparableItemDetails<TItem> _leftDetails, _rightDetails;

        private ISettableBroadcastingProperty<TItem> _selectedItem;
        public IBroadcastingProperty<TItem> SelectedItem { get; private set; }

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