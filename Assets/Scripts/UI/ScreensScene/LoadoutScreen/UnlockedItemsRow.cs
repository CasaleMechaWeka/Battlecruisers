using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class UnlockedItemsRow : MonoBehaviour 
	{
		public HorizontalLayoutGroup layoutGroup;
		public RectTransform scrollViewContent;

		public void Initialise(IUIFactory uiFactory, IList<Building> buildings)
		{
			Assert.IsNotNull(layoutGroup);
			Assert.IsNotNull(scrollViewContent);

			float totalWidth = 0;

			foreach (Building building in buildings)
			{
				UnlockedItem item = uiFactory.CreateUnlockedItem(layoutGroup, building);
				totalWidth += item.Size.x;
			}

			if (buildings.Count > 0)
			{
				totalWidth += (buildings.Count - 1) * layoutGroup.spacing;
			}

			scrollViewContent.sizeDelta = new Vector2(totalWidth, scrollViewContent.sizeDelta.y);
		}
	}
}
