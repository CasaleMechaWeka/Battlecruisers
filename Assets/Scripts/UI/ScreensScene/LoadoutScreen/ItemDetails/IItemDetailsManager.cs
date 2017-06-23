using BattleCruisers.Fetchers;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
	public class StateChangedEventArgs<TItem> : EventArgs where TItem : IComparableItem
	{
		public IItemDetailsState<TItem> NewState { get; private set; }

		public StateChangedEventArgs(IItemDetailsState<TItem> newState)
		{
			NewState = newState;
		}
	}

	public interface IItemDetailsManager<TItem> where TItem : IComparableItem
	{
		event EventHandler<StateChangedEventArgs<TItem>> StateChanged;

		void SelectItem(IItem<TItem> item);
		void ShowItemDetails(TItem item);
		void CompareSelectedItem();
		void CompareItemDetails(TItem item1, TItem item2);
		void HideItemDetails();
	}
}
