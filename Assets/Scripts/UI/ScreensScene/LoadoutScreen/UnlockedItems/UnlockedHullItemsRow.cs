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
		private HullItemsRow _hullsRow;
		private Cruiser _loadoutCruiser;
		private CruiserDetailsManager _detailsManager;
		private IList<UnlockedHullItem> _unlockedHullButtons;

		public void Initialise(HullItemsRow hullsRow, IUIFactory uiFactory, IList<Cruiser> unlockedCruisers, 
			Cruiser loadoutCruiser, CruiserDetailsManager detailsManager)
		{
			Assert.IsTrue(unlockedCruisers.Count > 0);

			_hullsRow = hullsRow;
			_loadoutCruiser = loadoutCruiser;
			_detailsManager = detailsManager;
			_unlockedHullButtons = new List<UnlockedHullItem>();

			_detailsManager.StateChanged += _detailsManager_StateChanged;

			base.Initialise(uiFactory, unlockedCruisers);
		}

		private void _detailsManager_StateChanged(object sender, StateChangedEventArgs<Cruiser> e)
		{
			// FELIX  Move to ItemDetails.States to avoid if/else if
			foreach (UnlockedHullItem unlockedHullButton in _unlockedHullButtons)
			{
				if (e.NewState.GetType() == typeof(DismissedState<Cruiser>))
				{
					unlockedHullButton.State = new DefaultCruiserState(_hullsRow);
				}
				else if (e.NewState.GetType() == typeof(ReadyToCompareState<Cruiser>))
				{
					unlockedHullButton.State = new ComparisonState<Cruiser>(_detailsManager);
				}
			}
		}

		protected override UnlockedItem<Cruiser> CreateUnlockedItem(Cruiser item, HorizontalOrVerticalLayoutGroup itemParent)
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
