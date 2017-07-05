using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
	public abstract class LoadoutItem<TItem> : BaseItem<TItem> where TItem : IComparableItem
	{
		protected IItemDetailsManager<TItem> _itemDetailsManager;

		private TItem _item;
		public override TItem Item 
		{
			get { return _item; }
			protected set
			{
				_item = value;
				itemImage.sprite = _item.Sprite;
			}
		}

		protected void InternalInitialise(TItem item, IItemDetailsManager<TItem> itemDetailsManager)
		{
			Item = item;
			_itemDetailsManager = itemDetailsManager;
		}

		public void SelectItem()
		{
			_itemDetailsManager.SelectItem(this);
		}
	}
}
