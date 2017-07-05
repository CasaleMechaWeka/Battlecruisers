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
	public class ComparisonState<TItem> : UnlockedItemState<TItem> where TItem : IComparableItem
	{
		private readonly IItemDetailsManager<TItem> _itemDetailsManager;

		protected override Color BackgroundColour { get { return BaseItem<TItem>.Colors.ENABLED; } }

		public ComparisonState(IItemDetailsManager<TItem> itemDetailsManager, UnlockedItem<TItem> item)
			: base(item)
		{
			_itemDetailsManager = itemDetailsManager;
		}

		public override void HandleSelection()
		{
			_itemDetailsManager.SelectItem(_item);
		}
	}
}
