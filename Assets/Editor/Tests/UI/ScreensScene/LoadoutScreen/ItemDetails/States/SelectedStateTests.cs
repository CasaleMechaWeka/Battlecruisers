using System;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreen.States
{
    public class SelectedStateTests : ItemDetailsStateTests
    {
        protected override IItemDetailsState<ICruiser> CreateItemState()
        {
            return new SelectedState<ICruiser>(_itemsDetailsManager, _itemStateManager, _selectedItem);
        }
		
        [Test]
		public void SelectItem_Throws()
        {
            Assert.Throws<InvalidProgramException>(() => _itemState.SelectItem(_selectedItem));
		}
		
        [Test]
        public void CompareSelectedItem_HidesDetails_ShowsSelectedFeedback_And_MovesToReadyToCompareState()
        {
            IItemDetailsState<ICruiser> nextState = _itemState.CompareSelectedItem();

            _itemsDetailsManager.Received().HideItemDetails();
            _selectedItem.Received().ShowSelectedFeedback = true;
            Assert.IsInstanceOf<ReadyToCompareState<ICruiser>>(nextState);
        }

        [Test]
        public void Dismiss_HidesDetails_And_MovesToDismissedState()
        {
            IItemDetailsState<ICruiser> nextState = _itemState.Dismiss();

            _itemsDetailsManager.Received().HideItemDetails();
            Assert.IsInstanceOf<DismissedState<ICruiser>>(nextState);
        }
    }
}
