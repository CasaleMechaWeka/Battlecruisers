using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems.States;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public abstract class UnlockedItem<TItem> : BaseItem<TItem> where TItem : IComparableItem
	{
		private RectTransform _rectTransform;

		public Image isInLoadoutFeedback;

		public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		public override TItem Item { get; protected set; }
		public IItemState<TItem> State { private get; set; }

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

		public void Initialise(IItemState<TItem> initialState, TItem item, bool isInLoadout)
		{
			Assert.IsNotNull(itemImage);
			Assert.IsNotNull(isInLoadoutFeedback);

			State = initialState;
			Item = item;
			IsItemInLoadout = isInLoadout;

			itemImage.sprite = item.Sprite;

			_rectTransform = transform.Parse<RectTransform>();
		}

		public void SelectItem()
		{
			State.HandleSelection();
		}
	}
}
