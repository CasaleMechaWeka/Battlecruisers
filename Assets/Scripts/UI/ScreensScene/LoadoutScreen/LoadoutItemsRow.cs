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

		public void Initialise(IUIFactory uiFactory, IList<Building> buildings)
		{
			// FELIX  Different limit for unlocked items :/
//			Assert.IsTrue(buildings.Count <= MAX_NUM_OF_ITEMS);

			foreach (Building building in buildings)
			{
				uiFactory.CreateLoadoutItem(layoutGroup, building);
			}
		}
	}
}
