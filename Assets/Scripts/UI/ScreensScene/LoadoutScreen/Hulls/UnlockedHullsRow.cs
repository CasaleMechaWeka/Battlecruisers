using BattleCruisers.Buildables.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Hulls
{
	public class UnlockedHullsRow : UnlockedItemsRow<Cruiser>
	{
		private HullsRow _hullsRow;
		private Cruiser _loadoutCruiser;
		private IList<UnlockedHullItem> _unlockedHullButtons;

		public void Initialise(HullsRow hullsRow, IUIFactory uiFactory, IList<Cruiser> unlockedCruisers, Cruiser loadoutCruiser)
		{
			Assert.IsTrue(unlockedCruisers.Count > 0);

			_hullsRow = hullsRow;
			_loadoutCruiser = loadoutCruiser;
			_unlockedHullButtons = new List<UnlockedHullItem>();

			base.Initialise(uiFactory, unlockedCruisers);
		}

		protected override UnlockedItem CreateUnlockedItem(Cruiser item, HorizontalOrVerticalLayoutGroup itemParent)
		{
			bool isInLoadout = object.ReferenceEquals(_loadoutCruiser, item);
			UnlockedHullItem unlockedHullItem = _uiFactory.CreateUnlockedHull(layoutGroup, _hullsRow, item, isInLoadout);
			_unlockedHullButtons.Add(unlockedHullItem);
			return unlockedHullItem;
		}

		public void UpdateSelectedHull(Cruiser selectedCruiser)
		{
			foreach (UnlockedHullItem unlockedHullButton in _unlockedHullButtons)
			{
				unlockedHullButton.OnNewHullSelected(selectedCruiser);
			}
		}
	}
}
