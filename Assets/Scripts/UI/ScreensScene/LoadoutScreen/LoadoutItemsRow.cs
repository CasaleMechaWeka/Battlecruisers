using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class LoadoutItemsRow : MonoBehaviour 
	{
		private const int MAX_NUM_OF_ITEMS = 5;

		public HorizontalLayoutGroup layoutGroup;
		public RectTransform scrollViewContent;

		public void Initialise(IUIFactory uiFactory, IList<Building> buildings)
		{
			// FELIX  Different limit for unlocked items :/
//			Assert.IsTrue(buildings.Count <= MAX_NUM_OF_ITEMS);

			float totalWidth = 0;

			foreach (Building building in buildings)
			{
				LoadoutItem item = uiFactory.CreateLoadoutItem(layoutGroup, building);
				totalWidth += item.Size.x;

				Debug.Log("totalWidth: " + totalWidth);
			}

			if (buildings.Count > 0)
			{
				totalWidth += (buildings.Count - 1) * layoutGroup.spacing;
			}
			
			Debug.Log("totalWidth: " + totalWidth);

			// FELIX  Do this in a nicer way.  Subclass for unlocked items?
			if (scrollViewContent != null)
			{
				scrollViewContent.sizeDelta = new Vector2(totalWidth, scrollViewContent.sizeDelta.y);
			}
		}
	}
}
