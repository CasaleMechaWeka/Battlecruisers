using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
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

		public bool ShowSelectedFeedback
		{
			set { selectedFeedbackImage.gameObject.SetActive(value); }
		}

		private bool _isItemInLoadout;
		protected bool IsItemInLoadout
		{
			get { return _isItemInLoadout; }
			set
			{
				_isItemInLoadout = value;
				isInLoadoutFeedback.gameObject.SetActive(_isItemInLoadout);
			}
		}

		protected void Initialise(TItem item, bool isInLoadout)
		{
			Assert.IsNotNull(itemImage);
			Assert.IsNotNull(isInLoadoutFeedback);

			Item = item;
			IsItemInLoadout = isInLoadout;

			_rectTransform = transform as RectTransform;
			Assert.IsNotNull(_rectTransform);
		}
	}
}
