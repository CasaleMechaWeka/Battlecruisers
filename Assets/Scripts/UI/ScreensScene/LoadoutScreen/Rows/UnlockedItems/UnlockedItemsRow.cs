using System.Collections.Generic;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public abstract class UnlockedItemsRow<TItem> : MonoBehaviour, IStatefulUIElement 
        where TItem : class, IComparableItem
    {
		protected IUIFactory _uiFactory;
		private IList<TItem> _unlockedItems;
        private int _numOfLockedItems;
        protected IItemsRow<TItem> _itemsRow;
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

        protected abstract UnlockedItem<TItem> CreateUnlockedItem(TItem item, HorizontalOrVerticalLayoutGroup itemParent);

        protected abstract LockedItem CreateLockedItem(HorizontalOrVerticalLayoutGroup itemParent);

        public void GoToState(UIState state)
        {
			foreach (UnlockedItem<TItem> unlockedItemButton in _unlockedItemButtons)
			{
				unlockedItemButton.GoToState(state);
			}
        }
	}
}
