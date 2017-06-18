using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public abstract class UnlockedItemsRow<TItem> : MonoBehaviour where TItem : ITarget
	{
		public HorizontalLayoutGroup layoutGroup;
		public RectTransform scrollViewContent;

		public void Initialise(IList<TItem> unlockedItems)
		{
			Assert.IsNotNull(layoutGroup);
			Assert.IsNotNull(scrollViewContent);

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
