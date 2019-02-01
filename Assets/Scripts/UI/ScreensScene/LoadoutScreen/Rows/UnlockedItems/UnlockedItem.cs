using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public abstract class UnlockedItem<TItem> : BaseItem<TItem> where TItem : class, IComparableItem
	{
        private IItemsRow<TItem> _itemsRow;

		public Image isInLoadoutFeedback;

		private bool _isItemInLoadout;
        public bool IsItemInLoadout
        {
            get { return _isItemInLoadout; }
            set
            {
                _isItemInLoadout = value;
                isInLoadoutFeedback.gameObject.SetActive(_isItemInLoadout);
            }
        }

        public void Initialise(
            TItem item, 
            IItemDetailsManager<TItem> itemDetailsManager,
            IItemsRow<TItem> itemsRow, 
            bool isInLoadout) 
        {
            base.Initialise(item, itemDetailsManager);

            Helper.AssertIsNotNull(itemsRow, isInLoadoutFeedback);

            _itemsRow = itemsRow;
            IsItemInLoadout = isInLoadout;

            GoToState(UIState.Default);
        }

        protected override IItemState<TItem> CreateDefaultState()
        {
            return new UnlockedItemDefaultState<TItem>(_itemsRow, this);
        }
	}
}
