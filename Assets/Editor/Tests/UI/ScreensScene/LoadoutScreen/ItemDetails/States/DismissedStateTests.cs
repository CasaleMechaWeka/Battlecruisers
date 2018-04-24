using System;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreen.States
{
    public class DismissedStateTests : ItemDetailsStateTests
    {
        protected override IItemDetailsState<ICruiser> CreateItemState()
        {
            return new DismissedState<ICruiser>(_itemsDetailsManager, _itemStateManager);
        }
        [Test]
        public void SelectItem_ShowsDetails_And_MovesToSelectedState()
        {
            IItemDetailsState<ICruiser> nextState = _itemState.SelectItem(_selectedItem);

            _itemsDetailsManager.Received().ShowItemDetails(_selectedItem.Item);
            Assert.IsInstanceOf<SelectedState<ICruiser>>(nextState);
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
