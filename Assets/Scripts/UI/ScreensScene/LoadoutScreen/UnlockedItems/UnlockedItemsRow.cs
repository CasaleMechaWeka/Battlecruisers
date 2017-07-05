using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	public abstract class UnlockedItemsRow<TItem> : MonoBehaviour where TItem : IComparableItem
	{
		protected IUIFactory _uiFactory;
		protected IItemsRow<TItem> _itemsRow;
		private IItemDetailsManager<TItem> _detailsManager;
		protected IList<UnlockedItem<TItem>> _unlockedItemButtons;

		public HorizontalLayoutGroup layoutGroup;
		public RectTransform scrollViewContent;

		public void Initialise(IUIFactory uiFactory, IList<TItem> unlockedItems, IItemsRow<TItem> itemsRow, IItemDetailsManager<TItem> detailsManager)
		{
			Assert.IsNotNull(layoutGroup);
			Assert.IsNotNull(scrollViewContent);

			_uiFactory = uiFactory;
			_itemsRow = itemsRow;
			_detailsManager = detailsManager;

			_detailsManager.StateChanged += _detailsManager_StateChanged;

			CreateUnockedItemButtons(unlockedItems);
		}

		private void CreateUnockedItemButtons(IList<TItem> unlockedItems)
		{
			_unlockedItemButtons = new List<UnlockedItem<TItem>>();
			float totalWidth = 0;

			foreach (TItem unlockedItem in unlockedItems)
			{
				UnlockedItem<TItem> itemButton = CreateUnlockedItem(unlockedItem, layoutGroup);
				_unlockedItemButtons.Add(itemButton);
				totalWidth += itemButton.Size.x;
			}

			if (unlockedItems.Count > 0)
			{
				totalWidth += (unlockedItems.Count - 1) * layoutGroup.spacing;
			}

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
	}
}
