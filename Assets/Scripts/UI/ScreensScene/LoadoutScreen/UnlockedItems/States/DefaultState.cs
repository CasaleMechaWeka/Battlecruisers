using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States
{
	public class DefaultState<TItem> : IUnlockedItemState<TItem> where TItem : IComparableItem
	{
		private readonly IItemsRow<TItem> _itemsRow;

		public DefaultState(IItemsRow<TItem> itemsRow)
		{
			_itemsRow = itemsRow;
		}

		public void HandleSelection(UnlockedItem<TItem> unlockedItem)
		{
			_itemsRow.SelectUnlockedItem(unlockedItem);
		}
	}
}
