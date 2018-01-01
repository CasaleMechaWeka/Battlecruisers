using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    public class UnlockedHullItemsRow : UnlockedItemsRow<ICruiser>
	{
		private ICruiser _loadoutCruiser;

		public void Initialise(
            HullItemsRow hullsRow, 
            IUIFactory uiFactory, 
            IList<ICruiser> unlockedCruisers, 
			ICruiser loadoutCruiser, 
            IItemDetailsManager<ICruiser> detailsManager)
		{
			Assert.IsTrue(unlockedCruisers.Count > 0);

			_loadoutCruiser = loadoutCruiser;

			base.Initialise(uiFactory, unlockedCruisers, hullsRow, detailsManager);
		}

		protected override UnlockedItem<ICruiser> CreateUnlockedItem(ICruiser item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isInLoadout = object.ReferenceEquals(_loadoutCruiser, item);
			return _uiFactory.CreateUnlockedHull(layoutGroup, _itemsRow, item, isInLoadout);
		}

		public void UpdateSelectedHull(ICruiser selectedCruiser)
		{
			foreach (UnlockedHullItem unlockedHullButton in _unlockedItemButtons)
			{
				unlockedHullButton.OnNewHullSelected(selectedCruiser);
			}
		}
	}
}
