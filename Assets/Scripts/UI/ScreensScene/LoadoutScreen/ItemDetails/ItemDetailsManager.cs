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
	public interface IItemDetailsManager<TItem> where TItem : IComparableItem
	{
		void SelectItem(LoadoutItem<TItem> item);
		void ShowItemDetails(TItem item);
		void CompareSelectedItem();
		void CompareItemDetails(TItem item1, TItem item2);
		void HideItemDetails();
	}

	public abstract class ItemDetailsManager<TItem> : MonoBehaviour, IItemDetailsManager<TItem>, IPointerClickHandler where TItem : IComparableItem
	{
		private IItemDetailsState<TItem> _state;
		private IComparableItemDetails<TItem> _singleItemDetails, _leftComparableItemDetails, _rightComparableItemDetails;

		public Image modalBackground;

		protected void Initialise(
			IComparableItemDetails<TItem> singleItemDetails,
			IComparableItemDetails<TItem> leftComparableItemDetails,
			IComparableItemDetails<TItem> rightComparableItemDetails)
		{
			_singleItemDetails = singleItemDetails;
			_leftComparableItemDetails = leftComparableItemDetails;
			_rightComparableItemDetails = rightComparableItemDetails;

			_state = new DismissedState<TItem>(this);
			
			HideItemDetails();
		}

		public void SelectItem(LoadoutItem<TItem> loadoutItem)
		{
			_state = _state.SelectItem(loadoutItem);
		}

		public void CompareSelectedItem()
		{
			_state = _state.CompareSelectedItem();
		}

		public void ShowItemDetails(TItem item)
		{
			modalBackground.gameObject.SetActive(true);
			_singleItemDetails.ShowItemDetails(item);
		}

		public void CompareItemDetails(TItem item1, TItem item2)
		{
			modalBackground.gameObject.SetActive(true);
			_leftComparableItemDetails.ShowItemDetails(item1, item2);
			_rightComparableItemDetails.ShowItemDetails(item2, item1);
		}

		public void HideItemDetails()
		{
			modalBackground.gameObject.SetActive(false);
			_singleItemDetails.Hide();
			_leftComparableItemDetails.Hide();
			_rightComparableItemDetails.Hide();
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			_state = _state.Dismiss();
		}
	}
}
