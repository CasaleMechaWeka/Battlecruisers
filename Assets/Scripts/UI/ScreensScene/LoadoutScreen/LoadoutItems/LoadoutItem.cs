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
	public abstract class LoadoutItem<TItem> : MonoBehaviour where TItem : IComparableItem
	{
		protected IItemDetailsManager<TItem> _itemDetailsManager;

		public Image itemImage;
		public Image selectedFeedbackImage;

		private TItem _item;
		public TItem Item 
		{
			get { return _item; }
			protected set
			{
				Assert.IsFalse(value.Equals(default(TItem)));
				_item = value;
				itemImage.sprite = _item.Sprite;
			}
		}

		public bool ShowSelectedFeedback
		{
			set { selectedFeedbackImage.gameObject.SetActive(value); }
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
