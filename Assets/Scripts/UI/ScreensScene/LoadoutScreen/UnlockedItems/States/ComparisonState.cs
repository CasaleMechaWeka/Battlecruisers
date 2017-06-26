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
	public class ComparisonState<TItem> : IUnlockedItemState<TItem> where TItem : IComparableItem
	{
		private readonly IItemDetailsManager<TItem> _itemDetailsManager;

		public ComparisonState(IItemDetailsManager<TItem> cruiserDetailsManager)
		{
			_itemDetailsManager = cruiserDetailsManager;
		}

		public void HandleSelection(UnlockedItem<TItem> item)
		{
			_itemDetailsManager.SelectItem(item);
		}
	}
}
