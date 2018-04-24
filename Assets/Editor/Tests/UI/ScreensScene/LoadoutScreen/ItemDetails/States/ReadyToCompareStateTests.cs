using System;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreen.States
{
    public class ReadyToCompareStateTests : ItemDetailsStateTests
    {
        protected override IItemDetailsState<ICruiser> CreateItemState()
        {
            return new ReadyToCompareState<ICruiser>(_itemsDetailsManager, _itemStateManager, _itemToCompare);
        }
		
        [Test]
        public void Constructor_CallsItemStateManager()
        {
            _itemToCompare.Type.Returns(ItemType.Unit);

            CreateItemState();

            _itemStateManager.Received().HandleDetailsManagerReadyToCompare(_itemToCompare.Type);
        }

        [Test]
		public void SelectItem_HidesShowSelectedFeedback_ComparesItemDetails_And_MovesToComparingState()
        {
            IItemDetailsState<ICruiser> nextState = _itemState.SelectItem(_selectedItem);

            _itemToCompare.Received().ShowSelectedFeedback = false;
            _itemsDetailsManager.Received().CompareItemDetails(_itemToCompare.Item, _selectedItem.Item);
            Assert.IsInstanceOf<ComparingState<ICruiser>>(nextState);
		}
		
        [Test]
        public void CompareSelectedItem_Throws()
        {
            Assert.Throws<InvalidProgramException>(() => _itemState.CompareSelectedItem());
        }

        [Test]
        public void Dismiss_Throws()
        {
            Assert.Throws<InvalidProgramException>(() => _itemState.Dismiss());
        }
    }
}
