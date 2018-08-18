using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedHullItemsRow : MonoBehaviour, IStatefulUIElement 
    {
        private ICruiser _loadoutHull;
        protected IUIFactory _uiFactory;
		private IList<ICruiser> _unlockedHulls;
        private int _numOfLockedItems;
        protected IItemsRow<ICruiser> _hullsRow;
		protected IList<UnlockedHullItem> _unlockedItemButtons;

		public HorizontalLayoutGroup layoutGroup;
		public RectTransform scrollViewContent;

        // FELIX  Add hull ot args?
        public void Initialise(IUnlockedItemsRowArgs<ICruiser> args, ICruiser loadoutHull)
		{
            Helper.AssertIsNotNull(layoutGroup, scrollViewContent, args, loadoutHull); 

			_uiFactory = args.UIFactory;
            _unlockedHulls = args.UnlockedItems;
            _numOfLockedItems = args.NumOfLockedItems;
			_hullsRow = args.ItemsRow;
            _loadoutHull = loadoutHull;
        }

        public void SetupUI()
        {
			CreateItemButtons(_unlockedHulls);
        }

        private void CreateItemButtons(IList<ICruiser> unlockedItems)
		{
            float unlockedItemsWidth = CreateUnlockedItems(unlockedItems);
            float lockedItemsWidth = CreatLockedItems();

            float rowWith = FindRowWidth(unlockedItems, unlockedItemsWidth, lockedItemsWidth);

			scrollViewContent.sizeDelta = new Vector2(rowWith, scrollViewContent.sizeDelta.y);
		}
		
		private float CreateUnlockedItems(IList<ICruiser> unlockedItems)
		{
			float width = 0;
			
			_unlockedItemButtons = new List<UnlockedHullItem>();
			
			foreach (ICruiser unlockedItem in unlockedItems)
			{
				UnlockedHullItem itemButton = CreateUnlockedItem(unlockedItem, layoutGroup);
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

        private float FindRowWidth(IList<ICruiser> unlockedItems, float unlockedItemsWidth, float lockedItemsWidth)
        {
            int numOfItems = unlockedItems.Count + _numOfLockedItems;
            Assert.IsTrue(numOfItems > 0);
            float spacesWidth = (numOfItems - 1) * layoutGroup.spacing;
            return unlockedItemsWidth + lockedItemsWidth + spacesWidth;
        }

        private UnlockedHullItem CreateUnlockedItem(ICruiser hull, HorizontalOrVerticalLayoutGroup itemParent)
        {
            bool isInLoadout = ReferenceEquals(_loadoutHull, hull);
            return _uiFactory.CreateUnlockedHull(layoutGroup, _hullsRow, hull, isInLoadout);
        }

        private LockedItem CreateLockedItem(HorizontalOrVerticalLayoutGroup itemParent)
        {
            return _uiFactory.CreateLockedHull(itemParent);
        }

        public void GoToState(UIState state)
        {
			foreach (UnlockedHullItem unlockedItemButton in _unlockedItemButtons)
			{
				unlockedItemButton.GoToState(state);
			}
        }

        public void UpdateSelectedHull(ICruiser selectedHull)
        {
            foreach (UnlockedHullItem unlockedHullButton in _unlockedItemButtons)
            {
                unlockedHullButton.OnNewHullSelected(selectedHull);
            }
        }
    }
}
