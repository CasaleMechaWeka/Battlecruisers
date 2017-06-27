using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
	public class LoadoutBuildableItemsRow : MonoBehaviour 
	{
		private IUIFactory _uiFactory;
		private IDictionary<Building, LoadoutBuildingItem> _buildingToLoadoutItem;

		private const int MAX_NUM_OF_ITEMS = 3;

		public HorizontalLayoutGroup layoutGroup;

		public void Initialise(IUIFactory uiFactory, IList<Building> buildings)
		{
			Assert.IsNotNull(layoutGroup);
			Assert.IsTrue(buildings.Count <= MAX_NUM_OF_ITEMS);
			Assert.IsNotNull(uiFactory);
			Assert.IsNotNull(buildings);

			_uiFactory = uiFactory;
			_buildingToLoadoutItem = new Dictionary<Building, LoadoutBuildingItem>();

			foreach (Building building in buildings)
			{
				CreateLoadoutItem(building);
			}
		}

		public bool CanAddBuilding()
		{
			return _buildingToLoadoutItem.Count < MAX_NUM_OF_ITEMS;
		}

		public void AddBuilding(Building buildingToAdd)
		{
			// FELIX  Handle if already filled all slots :P
			// FELIX  Perhaps expose CanAdd property?

			CreateLoadoutItem(buildingToAdd);
		}

		public void RemoveBuilding(Building buildingToRemove)
		{
			RemoveLoadoutItem(buildingToRemove);
		}
		
		private void CreateLoadoutItem(Building buildingToAdd)
		{
			Assert.IsFalse(_buildingToLoadoutItem.ContainsKey(buildingToAdd));
			LoadoutBuildingItem item = _uiFactory.CreateLoadoutItem(layoutGroup, buildingToAdd);
			_buildingToLoadoutItem.Add(buildingToAdd, item);
		}
		
		private void RemoveLoadoutItem(Building buildingToRemove)
		{
			Assert.IsTrue(_buildingToLoadoutItem.ContainsKey(buildingToRemove));
			LoadoutBuildingItem item = _buildingToLoadoutItem[buildingToRemove];
			_buildingToLoadoutItem.Remove(buildingToRemove);
			Destroy(item.gameObject);
		}
	}
}
