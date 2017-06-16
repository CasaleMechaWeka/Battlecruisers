using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public abstract class UnlockedItem : MonoBehaviour 
	{
		private RectTransform _rectTransform;

		public Image itemImage;
		public Image isInLoadoutFeedback;

		public Vector2 Size { get { return _rectTransform.sizeDelta; } }

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

		protected void Initialise(bool isInLoadout)
		{
			Assert.IsNotNull(itemImage);
			Assert.IsNotNull(isInLoadoutFeedback);

			IsItemInLoadout = isInLoadout;

			_rectTransform = transform as RectTransform;
			Assert.IsNotNull(_rectTransform);
		}
	}
}
