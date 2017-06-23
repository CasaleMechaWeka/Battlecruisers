using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States
{
	// FELIX   Move to own class
	public interface IUnlockedItemState
	{
		// FELIX  Item background colour!

		void HandleSelection();
	}

	public class DefaultState : IUnlockedItemState
	{
		private readonly HullItemsRow _hullItemsRow;

		public DefaultState(HullItemsRow hullItemsRow)
		{
			_hullItemsRow = hullItemsRow;
		}

		public void HandleSelection(Cruiser hull)
		{
			_hullItemsRow.SelectHull(hull);
		}
	}

	// FELIX   Move to own class
	public class ComparisonState
	{
		private readonly CruiserDetailsManager _cruiserDetailsManager;

		public ComparisonState(CruiserDetailsManager cruiserDetailsManager)
		{
			_cruiserDetailsManager = cruiserDetailsManager;
		}

		public void HandleSelection(Cruiser hull)
		{
//			_cruiserDetailsManager.SelectItem(
		}
	}
}
