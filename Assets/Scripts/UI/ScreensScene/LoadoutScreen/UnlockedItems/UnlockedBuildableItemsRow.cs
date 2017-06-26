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
	public class UnlockedBuildableItemsRow : UnlockedItemsRow<Building>
	{
		private BuildableItemsRow _itemsRow;
		private IList<Building> _loadoutBuildings;
		private BuildableDetailsManager _detailsManager;
		private IList<UnlockedBuildableItem> _unlockedBuildableButtons;

		public void Initialise(BuildableItemsRow itemsRow, IUIFactory uiFactory, IList<Building> unlockedBuildings, 
			IList<Building> loadoutBuildings, BuildableDetailsManager detailsManager)
		{
			_itemsRow = itemsRow;
			_uiFactory = uiFactory;
			_loadoutBuildings = loadoutBuildings;
			_detailsManager = detailsManager;
			_unlockedBuildableButtons = new List<UnlockedBuildableItem>();

			_detailsManager.StateChanged += _detailsManager_StateChanged;

			base.Initialise(uiFactory, unlockedBuildings);
		}

		private void _detailsManager_StateChanged(object sender, StateChangedEventArgs<Buildable> e)
		{
			foreach (UnlockedBuildableItem unlockedBuildableButton in _unlockedBuildableButtons)
			{
				if (e.NewState.IsInReadyToCompareState)
				{
					unlockedBuildableButton.State = new ComparisonState<Buildable>(_detailsManager);
				}
				else
				{
					unlockedBuildableButton.State = new DefaultState<Buildable>(_itemsRow);
				}
			}	
		}

		protected override UnlockedItem<Building> CreateUnlockedItem(Building item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isBuildingInLoadout = _loadoutBuildings.Contains(item);
			UnlockedBuildableItem unlockedBuildableItem = _uiFactory.CreateUnlockedBuildableItem(layoutGroup, _itemsRow, item, isBuildingInLoadout);
			_unlockedBuildableButtons.Add(unlockedBuildableItem);
			return unlockedBuildableItem;
		}
	}
}
