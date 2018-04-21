using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public abstract class UnlockedItem<TItem> : BaseItem<TItem> where TItem : IComparableItem
	{
        private IItemsRow<TItem> _itemsRow;
		private RectTransform _rectTransform;

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

        public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		
        public void Initialise(
            TItem item, 
            IItemDetailsManager<TItem> itemDetailsManager,
            IItemsRow<TItem> itemsRow, 
            bool isInLoadout) 
        {
            base.Initialise(item, itemDetailsManager);

            Helper.AssertIsNotNull(itemsRow, isInLoadoutFeedback);

            _itemsRow = itemsRow;
            _rectTransform = transform.Parse<RectTransform>();
            IsItemInLoadout = isInLoadout;

            GoToDefaultState();
        }

        public sealed override void GoToDefaultState()
        {
            _state = new UnlockedItemDefaultState<TItem>(_itemsRow, this);
        }
	}
}
