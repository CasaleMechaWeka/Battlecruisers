using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public abstract class ItemDetailsManager<TItem> : PresentableController, IItemDetailsManager<TItem>, IPointerClickHandler 
        where TItem : IComparableItem
	{
		private IComparableItemDetails<TItem> _singleItemDetails, _leftComparableItemDetails, _rightComparableItemDetails;
        private IItemStateManager _itemStateManager;
        private IItemDetailsState<TItem> _defaultState;

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
			IComparableItemDetails<TItem> rightComparableItemDetails,
            IItemStateManager itemStateManager)
		{
            base.Initialise();

            Helper.AssertIsNotNull(singleItemDetails, leftComparableItemDetails, rightComparableItemDetails, itemStateManager);

			_singleItemDetails = singleItemDetails;
			_leftComparableItemDetails = leftComparableItemDetails;
			_rightComparableItemDetails = rightComparableItemDetails;
            _itemStateManager = itemStateManager;

            _defaultState = new DismissedState<TItem>(this, itemStateManager);
            State = _defaultState;
			
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

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);

            State = _defaultState;
            _itemStateManager.HandleDetailsManagerDismissed();
        }
	}
}
