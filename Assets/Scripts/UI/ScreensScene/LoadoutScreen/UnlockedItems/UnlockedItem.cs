using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	public abstract class UnlockedItem<TItem> : MonoBehaviour, IItem<TItem> where TItem : IComparableItem
	{
		private RectTransform _rectTransform;

		public Image itemImage;
		public Image isInLoadoutFeedback;
		public Image selectedFeedbackImage;

		public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		public TItem Item { get; protected set; }
		public IUnlockedItemState<TItem> State { private get; set; }

		public bool ShowSelectedFeedback
		{
			set { selectedFeedbackImage.gameObject.SetActive(value); }
		}

		private bool _isItemInLoadout;
		public bool IsItemInLoadout
		{
			get { return _isItemInLoadout; }
			set
			{
				_isItemInLoadout = value;
				isInLoadoutFeedback.gameObject.SetActive(_isItemInLoadout);
			}
		}

		public void Initialise(IUnlockedItemState<TItem> initialState, TItem item, bool isInLoadout)
		{
			Assert.IsNotNull(itemImage);
			Assert.IsNotNull(isInLoadoutFeedback);

			State = initialState;
			Item = item;
			IsItemInLoadout = isInLoadout;

			itemImage.sprite = item.Sprite;

			_rectTransform = transform as RectTransform;
			Assert.IsNotNull(_rectTransform);
		}

		public void SelectItem()
		{
			State.HandleSelection(this);
		}
	}
}
