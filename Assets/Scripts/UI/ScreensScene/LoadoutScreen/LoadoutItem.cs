using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public interface IComparableItem
	{
		Sprite Sprite { get; }
	}

	public abstract class LoadoutItem<TItem> : MonoBehaviour where TItem : IComparableItem
	{
		protected IItemDetailsManager<TItem> _itemDetailsManager;

		public Image itemImage;
		public Image selectedFeedbackImage;

		public TItem Item { private set; get; }

		public bool ShowSelectedFeedback
		{
			set { selectedFeedbackImage.gameObject.SetActive(value); }
		}

		public void Initialise(TItem item, IItemDetailsManager<TItem> itemDetailsManager)
		{
			Item = item;
			itemImage.sprite = item.Sprite;
			_itemDetailsManager = itemDetailsManager;
		}

		public void SelectItem()
		{
			_itemDetailsManager.SelectItem(this);
		}
	}
}
