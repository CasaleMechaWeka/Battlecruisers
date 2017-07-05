using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.Utils;
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
		private readonly UnlockedItem<TItem> _item;

		public DefaultState(IItemsRow<TItem> itemsRow, UnlockedItem<TItem> item)
		{
			_itemsRow = itemsRow;
			_item = item;
			_item.backgroundImage.color = BaseItem<TItem>.Colors.DEFAULT;
		}

		public void HandleSelection()
		{
			_item.IsItemInLoadout = _itemsRow.SelectUnlockedItem(_item);
		}
	}
}
