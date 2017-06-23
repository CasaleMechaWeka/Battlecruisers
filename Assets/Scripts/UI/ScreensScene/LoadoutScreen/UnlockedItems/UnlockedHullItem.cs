using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	public class UnlockedHullItem : UnlockedItem<Cruiser>
	{
		public IUnlockedItemState<Cruiser> State { private get; set; }

		public void Initialise(IUnlockedItemState<Cruiser> initialState, Cruiser cruiser, bool isInLoadout)
		{
			base.Initialise(cruiser, isInLoadout);

			itemImage.sprite = Item.Sprite;
			State = initialState;
		}

		public void SelectHull()
		{
			State.HandleSelection(this);
		}

		public void OnNewHullSelected(Cruiser selectedCruiser)
		{
			IsItemInLoadout = object.ReferenceEquals(selectedCruiser, Item);
		}
	}
}
