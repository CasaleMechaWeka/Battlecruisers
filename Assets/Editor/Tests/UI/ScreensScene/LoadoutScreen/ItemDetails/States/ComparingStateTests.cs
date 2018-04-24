using System;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreen.States
{
    public class ComparingStateTests : ItemDetailsStateTests
    {
        protected override IItemDetailsState<ICruiser> CreateItemState()
        {
            return new ComparingState<ICruiser>(_itemsDetailsManager, _itemStateManager);
        }
		
        [Test]
        public void Constructor_CallsItemStateManager()
        {
            CreateItemState();
            _itemStateManager.Received().HandleDetailsManagerComparing();
        }

        [Test]
		public void SelectItem_Throws()
        {
            Assert.Throws<InvalidProgramException>(() => _itemState.SelectItem(_selectedItem));
		}
		
        [Test]
        public void CompareSelectedItem_Throws()
        {
            Assert.Throws<InvalidProgramException>(() => _itemState.CompareSelectedItem());
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
