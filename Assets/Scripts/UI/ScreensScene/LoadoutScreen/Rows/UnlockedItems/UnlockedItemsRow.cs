using System.Collections.Generic;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates;
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

        private void CreateItemButtons(IList<TItem> unlockedItems)
		{
            float unlockedItemsWidth = CreateUnlockedItems(unlockedItems);
            float lockedItemsWidth = CreatLockedItems();

            float rowWith = FindRowWidth(unlockedItems, unlockedItemsWidth, lockedItemsWidth);

			scrollViewContent.sizeDelta = new Vector2(rowWith, scrollViewContent.sizeDelta.y);
		}
		
		private float CreateUnlockedItems(IList<TItem> unlockedItems)
		{
			float width = 0;
			
			_unlockedItemButtons = new List<UnlockedItem<TItem>>();
			
			foreach (TItem unlockedItem in unlockedItems)
			{
				UnlockedItem<TItem> itemButton = CreateUnlockedItem(unlockedItem, layoutGroup);
				_unlockedItemButtons.Add(itemButton);
				width += itemButton.Size.x;
			}
			
			return width;
		}

        private float CreatLockedItems()
        {
            float width = 0;

            for (int i = 0; i < _numOfLockedItems; ++i)
            {
                LockedItem lockedItem = CreateLockedItem(layoutGroup);
                width += lockedItem.Size.x;
            }

            return width;
        }

        private float FindRowWidth(IList<TItem> unlockedItems, float unlockedItemsWidth, float lockedItemsWidth)
        {
            int numOfItems = unlockedItems.Count + _numOfLockedItems;
            Assert.IsTrue(numOfItems > 0);
            float spacesWidth = (numOfItems - 1) * layoutGroup.spacing;
            return unlockedItemsWidth + lockedItemsWidth + spacesWidth;
        }

        // FELIX  Will be replaced :)
        private void _detailsManager_StateChanged(object sender, StateChangedEventArgs<TItem> e)
		{
			foreach (UnlockedItem<TItem> unlockedItemButton in _unlockedItemButtons)
			{
				if (e.NewState.IsInReadyToCompareState)
				{
					unlockedItemButton.State = new HighlightedState<TItem>(_detailsManager, unlockedItemButton);
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
