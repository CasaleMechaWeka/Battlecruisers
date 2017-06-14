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
		private IList<Building> _buildings;

		private const int MAX_NUM_OF_ITEMS = 5;

		public HorizontalLayoutGroup layoutGroup;

		public void Initialise(IUIFactory uiFactory, IList<Building> buildings)
		{
			Assert.IsNotNull(layoutGroup);
			Assert.IsTrue(buildings.Count <= MAX_NUM_OF_ITEMS);
			Assert.IsNotNull(uiFactory);
			Assert.IsNotNull(buildings);

			_buildings = buildings;

			foreach (Building building in _buildings)
			{
				uiFactory.CreateLoadoutItem(layoutGroup, building);
			}
		}

		public void AddBuilding(Building buildingToAdd)
		{
			Assert.IsFalse(_buildings.Contains(buildingToAdd));
		}

		public void RemoveBuilding(Building buildingToRemove)
		{
			Assert.IsTrue(_buildings.Contains(buildingToRemove));

		}
	}
}
