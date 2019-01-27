using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    // FELIX  Test :D
    // FELIX  Rename to ItemDetailsDisplayer?
    public class ItemFamilyDetailsDisplayer<TItem> : IItemFamilyDetailsDisplayer<TItem> where TItem : class, IComparableItem
    {
        private readonly IComparableItemDetails<TItem> _leftDetails, _rightDetails;
        private TItem _selectedItem;

        public ItemFamilyDetailsDisplayer(IComparableItemDetails<TItem> leftDetails, IComparableItemDetails<TItem> rightDetails)
        {
            Helper.AssertIsNotNull(leftDetails, rightDetails);

            _leftDetails = leftDetails;
            _rightDetails = rightDetails;
            _selectedItem = null;
        }

        public void SelectItem(TItem item)
        {
            Assert.IsNotNull(item);

            HideDetails();
            _selectedItem = item;
            _leftDetails.ShowItemDetails(item);
        }

        public void CompareWithSelectedItem(TItem item)
        {
            Assert.IsNotNull(item);
            Assert.IsNotNull(_selectedItem);

            _leftDetails.ShowItemDetails(_selectedItem, item);
            _rightDetails.ShowItemDetails(item, _selectedItem);
        }

        public void HideDetails()
        {
            _leftDetails.Hide();
            _rightDetails.Hide();
            _selectedItem = null;
        }
    }
}