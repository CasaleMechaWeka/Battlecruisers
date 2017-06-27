using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	// FELIX  Avoid duplciate code with sister class
	public class UnlockedBuildingItemsRow : UnlockedItemsRow<Building>
	{
		private BuildingItemsRow _itemsRow;
		private IList<Building> _loadoutBuildings;
		private BuildingDetailsManager _detailsManager;
		private IList<UnlockedBuildingItem> _unlockedBuildingButtons;

		public void Initialise(BuildingItemsRow itemsRow, IUIFactory uiFactory, IList<Building> unlockedBuildings, 
			IList<Building> loadoutBuildings, BuildingDetailsManager detailsManager)
		{
			_itemsRow = itemsRow;
			_uiFactory = uiFactory;
			_loadoutBuildings = loadoutBuildings;
			_detailsManager = detailsManager;
			_unlockedBuildingButtons = new List<UnlockedBuildingItem>();

			_detailsManager.StateChanged += _detailsManager_StateChanged;

			base.Initialise(uiFactory, unlockedBuildings);
		}

		private void _detailsManager_StateChanged(object sender, StateChangedEventArgs<Building> e)
		{
			foreach (UnlockedBuildingItem unlockedBuildableButton in _unlockedBuildingButtons)
			{
				if (e.NewState.IsInReadyToCompareState)
				{
					unlockedBuildableButton.State = new ComparisonState<Building>(_detailsManager);
				}
				else
				{
					unlockedBuildableButton.State = new DefaultState<Building>(_itemsRow);
				}
			}	
		}

		protected override UnlockedItem<Building> CreateUnlockedItem(Building item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isBuildingInLoadout = _loadoutBuildings.Contains(item);
			UnlockedBuildingItem unlockedBuildableItem = _uiFactory.CreateUnlockedBuildableItem(layoutGroup, _itemsRow, item, isBuildingInLoadout);
			_unlockedBuildingButtons.Add(unlockedBuildableItem);
			return unlockedBuildableItem;
		}
	}
}
