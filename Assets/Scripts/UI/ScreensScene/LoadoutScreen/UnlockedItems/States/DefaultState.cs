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
	// FELIX   Move to own class
	public interface IUnlockedItemState<TItem> where TItem : IComparableItem
	{
		// FELIX  Item background colour!

		void HandleSelection(UnlockedItem<TItem> item);
	}

	// FELIX  Make generic class for Buildables/Hulls?
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

	// FELIX   Move to own class
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
