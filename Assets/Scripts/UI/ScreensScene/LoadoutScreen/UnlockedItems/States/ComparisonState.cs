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
	// FELIX  Base state class?
	public class ComparisonState<TItem> : IUnlockedItemState<TItem> where TItem : IComparableItem
	{
		private readonly IItemDetailsManager<TItem> _itemDetailsManager;
		private readonly UnlockedItem<TItem> _item;

		private readonly static Color BACKGROUND_COLOR = Color.green;

		public ComparisonState(IItemDetailsManager<TItem> itemDetailsManager, UnlockedItem<TItem> item)
		{
			_itemDetailsManager = itemDetailsManager;
			_item = item;
			_item.backgroundImage.color = BACKGROUND_COLOR;
		}

		public void HandleSelection()
		{
			_itemDetailsManager.SelectItem(_item);
		}
	}
}
