using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	public class UnlockedHullItemsRow : UnlockedItemsRow<Cruiser>
	{
		private Cruiser _loadoutCruiser;

		public void Initialise(HullItemsRow hullsRow, IUIFactory uiFactory, IList<Cruiser> unlockedCruisers, 
			Cruiser loadoutCruiser, CruiserDetailsManager detailsManager)
		{
			Assert.IsTrue(unlockedCruisers.Count > 0);

			_loadoutCruiser = loadoutCruiser;

			base.Initialise(uiFactory, unlockedCruisers, hullsRow, detailsManager);
		}

		protected override UnlockedItem<Cruiser> CreateUnlockedItem(Cruiser item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isInLoadout = object.ReferenceEquals(_loadoutCruiser, item);
			return _uiFactory.CreateUnlockedHull(layoutGroup, _itemsRow, item, isInLoadout);
		}

		public void UpdateSelectedHull(Cruiser selectedCruiser)
		{
			foreach (UnlockedHullItem unlockedHullButton in _unlockedItemButtons)
			{
				unlockedHullButton.OnNewHullSelected(selectedCruiser);
			}
		}
	}
}
