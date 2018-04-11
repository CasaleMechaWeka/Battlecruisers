using System.Collections.Generic;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems.States;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public abstract class UnlockedItemsRow<TItem> : MonoBehaviour where TItem : IComparableItem
	{
		protected IUIFactory _uiFactory;
		private IList<TItem> _unlockedItems;
        private int _numOfLockedItems;
        protected IItemsRow<TItem> _itemsRow;
        private IItemDetailsManager<TItem> _detailsManager;
		protected IList<UnlockedItem<TItem>> _unlockedItemButtons;

		public HorizontalLayoutGroup layoutGroup;
		public RectTransform scrollViewContent;

        public void Initialise(IUnlockedItemsRowArgs<TItem> args)
		{
            Helper.AssertIsNotNull(layoutGroup, scrollViewContent, args); 

			_uiFactory = args.UIFactory;
            _unlockedItems = args.UnlockedItems;
            _numOfLockedItems = args.NumOfLockedItems;
			_itemsRow = args.ItemsRow;
			_detailsManager = args.DetailsManager;

			_detailsManager.StateChanged += _detailsManager_StateChanged;
        }

        public void SetupUI()
        {
			CreateItemButtons(_unlockedItems);
        }

        // FELIX  Split into smaller methods
        private void CreateItemButtons(IList<TItem> unlockedItems)
		{
            // Create unlocked items
			_unlockedItemButtons = new List<UnlockedItem<TItem>>();
			float totalWidth = 0;

			foreach (TItem unlockedItem in unlockedItems)
			{
				UnlockedItem<TItem> itemButton = CreateUnlockedItem(unlockedItem, layoutGroup);
				_unlockedItemButtons.Add(itemButton);
				totalWidth += itemButton.Size.x;
			}

            // Create locked items
            for (int i = 0; i < _numOfLockedItems; ++i)
            {
                LockedItem lockedItem = CreateLockedItem(layoutGroup);
                totalWidth += lockedItem.Size.x;
            }

            int numOfItems = unlockedItems.Count + _numOfLockedItems;
            Assert.IsTrue(numOfItems > 0);
            totalWidth += (numOfItems - 1) * layoutGroup.spacing;

			scrollViewContent.sizeDelta = new Vector2(totalWidth, scrollViewContent.sizeDelta.y);
		}

		private void _detailsManager_StateChanged(object sender, StateChangedEventArgs<TItem> e)
		{
			foreach (UnlockedItem<TItem> unlockedItemButton in _unlockedItemButtons)
			{
				if (e.NewState.IsInReadyToCompareState)
				{
					unlockedItemButton.State = new ComparisonState<TItem>(_detailsManager, unlockedItemButton);
				}
				else
				{
					unlockedItemButton.State = new DefaultState<TItem>(_itemsRow, unlockedItemButton);
				}
			}
		}

		protected abstract UnlockedItem<TItem> CreateUnlockedItem(TItem item, HorizontalOrVerticalLayoutGroup itemParent);

        protected abstract LockedItem CreateLockedItem(HorizontalOrVerticalLayoutGroup itemParent);
	}
}
