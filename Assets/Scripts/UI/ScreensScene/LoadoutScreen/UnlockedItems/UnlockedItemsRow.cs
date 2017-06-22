using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	public abstract class UnlockedItemsRow<TItem> : MonoBehaviour where TItem : ITarget
	{
		protected IUIFactory _uiFactory;

		public HorizontalLayoutGroup layoutGroup;
		public RectTransform scrollViewContent;

		public void Initialise(IUIFactory uiFactory, IList<TItem> unlockedItems)
		{
			Assert.IsNotNull(layoutGroup);
			Assert.IsNotNull(scrollViewContent);

			_uiFactory = uiFactory;

			float totalWidth = 0;

			foreach (TItem unlockedItem in unlockedItems)
			{
				UnlockedItem item = CreateUnlockedItem(unlockedItem, layoutGroup);
				totalWidth += item.Size.x;
			}

			if (unlockedItems.Count > 0)
			{
				totalWidth += (unlockedItems.Count - 1) * layoutGroup.spacing;
			}

			scrollViewContent.sizeDelta = new Vector2(totalWidth, scrollViewContent.sizeDelta.y);
		}

		protected abstract UnlockedItem CreateUnlockedItem(TItem item, HorizontalOrVerticalLayoutGroup itemParent);
	}
}
