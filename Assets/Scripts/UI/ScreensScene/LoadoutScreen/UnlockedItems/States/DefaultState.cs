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

		void HandleSelection(IItem<TItem> item);
	}

	// FELIX  Make generic class for Buildables/Hulls?
	public class DefaultCruiserState : IUnlockedItemState<Cruiser>
	{
		private readonly HullItemsRow _hullItemsRow;

		public DefaultCruiserState(HullItemsRow hullItemsRow)
		{
			_hullItemsRow = hullItemsRow;
		}

		public void HandleSelection(IItem<Cruiser> hullItem)
		{
			_hullItemsRow.SelectHull(hullItem.Item);
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

		public void HandleSelection(IItem<TItem> item)
		{
			_itemDetailsManager.SelectItem(item);
		}
	}
}
