using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems
{
    public class LoadoutBuildingItemsRow : MonoBehaviour 
	{
		private IUIFactory _uiFactory;
		private IItemDetailsManager<IBuilding> _detailsManager;
		private IDictionary<IBuilding, LoadoutBuildingItem> _buildingToLoadoutItem;

		private const int MAX_NUM_OF_ITEMS = 4;

		public HorizontalLayoutGroup layoutGroup;

		public void Initialise(IUIFactory uiFactory, IList<IBuilding> buildings, IItemDetailsManager<IBuilding> detailsManager)
		{
			Assert.IsNotNull(layoutGroup);
			Assert.IsTrue(buildings.Count <= MAX_NUM_OF_ITEMS);
			Assert.IsNotNull(uiFactory);
			Assert.IsNotNull(detailsManager);
			Assert.IsNotNull(buildings);

			_uiFactory = uiFactory;
			_detailsManager = detailsManager;
			_buildingToLoadoutItem = new Dictionary<IBuilding, LoadoutBuildingItem>();

			foreach (Building building in buildings)
			{
				CreateLoadoutItem(building);
			}

			_detailsManager.StateChanged += _detailsManager_StateChanged;
		}

		private void _detailsManager_StateChanged(object sender, StateChangedEventArgs<IBuilding> e)
		{
			foreach (LoadoutBuildingItem item in _buildingToLoadoutItem.Values)
			{
				item.backgroundImage.color = e.NewState.IsInReadyToCompareState ? BaseItem<Building>.Colors.ENABLED : BaseItem<Building>.Colors.DEFAULT;
			}
		}

		public bool CanAddBuilding()
		{
			return _buildingToLoadoutItem.Count < MAX_NUM_OF_ITEMS;
		}

		public void AddBuilding(IBuilding buildingToAdd)
		{
			// FELIX  Handle if already filled all slots :P
			// Perhaps expose CanAdd property?

			CreateLoadoutItem(buildingToAdd);
		}

		public void RemoveBuilding(IBuilding buildingToRemove)
		{
			RemoveLoadoutItem(buildingToRemove);
		}
		
		private void CreateLoadoutItem(IBuilding buildingToAdd)
		{
			Assert.IsFalse(_buildingToLoadoutItem.ContainsKey(buildingToAdd));
			LoadoutBuildingItem item = _uiFactory.CreateLoadoutItem(layoutGroup, buildingToAdd);
			_buildingToLoadoutItem.Add(buildingToAdd, item);
		}
		
		private void RemoveLoadoutItem(IBuilding buildingToRemove)
		{
			Assert.IsTrue(_buildingToLoadoutItem.ContainsKey(buildingToRemove));
			LoadoutBuildingItem item = _buildingToLoadoutItem[buildingToRemove];
			_buildingToLoadoutItem.Remove(buildingToRemove);
			Destroy(item.gameObject);
		}
	}
}
