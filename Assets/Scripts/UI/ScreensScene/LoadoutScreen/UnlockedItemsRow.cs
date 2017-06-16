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
		private IList<UnlockedBuildableItem> _items;

		public HorizontalLayoutGroup layoutGroup;
		public RectTransform scrollViewContent;

		public void Initialise(ItemsRow itemsRow, IUIFactory uiFactory, IList<Building> unlockedBuildings, IList<Building> loadoutBuildings)
		{
			Assert.IsNotNull(layoutGroup);
			Assert.IsNotNull(scrollViewContent);

			_items = new List<UnlockedBuildableItem>();
			float totalWidth = 0;

			foreach (Building unlockedBuilding in unlockedBuildings)
			{
				bool isBuildingInLoadout = loadoutBuildings.Contains(unlockedBuilding);
				UnlockedBuildableItem item = uiFactory.CreateUnlockedItem(layoutGroup, itemsRow, unlockedBuilding, isBuildingInLoadout);
				_items.Add(item);
				totalWidth += item.Size.x;
			}

			if (unlockedBuildings.Count > 0)
			{
				totalWidth += (unlockedBuildings.Count - 1) * layoutGroup.spacing;
			}

			scrollViewContent.sizeDelta = new Vector2(totalWidth, scrollViewContent.sizeDelta.y);
		}
	}
}
