using System;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public abstract class ItemDetailsManager<TItem> : MonoBehaviour, IItemDetailsManager<TItem>, IPointerClickHandler where TItem : IComparableItem
	{
		private IComparableItemDetails<TItem> _singleItemDetails, _leftComparableItemDetails, _rightComparableItemDetails;

		public Image modalBackground;

		public event EventHandler<StateChangedEventArgs<TItem>> StateChanged;
		private IItemDetailsState<TItem> _state;
		
		private IItemDetailsState<TItem> State
		{
			get { return _state; }
			set
			{
				_state = value;
				if (StateChanged != null)
				{
					StateChanged.Invoke(this, new StateChangedEventArgs<TItem>(_state));
				}
			}
		}

		protected void Initialise(
			IComparableItemDetails<TItem> singleItemDetails,
			IComparableItemDetails<TItem> leftComparableItemDetails,
			IComparableItemDetails<TItem> rightComparableItemDetails)
		{
			_singleItemDetails = singleItemDetails;
			_leftComparableItemDetails = leftComparableItemDetails;
			_rightComparableItemDetails = rightComparableItemDetails;

			State = new DismissedState<TItem>(this);
			
			HideItemDetails();
		}

		public void SelectItem(IItem<TItem> item)
		{
			State = State.SelectItem(item);
		}

		public void CompareSelectedItem()
		{
			State = State.CompareSelectedItem();
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
			State = State.Dismiss();
		}
	}
}
