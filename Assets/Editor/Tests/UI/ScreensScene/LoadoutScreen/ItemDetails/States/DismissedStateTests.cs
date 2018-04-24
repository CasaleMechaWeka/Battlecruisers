using System;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreen.States
{
    public class DismissedStateTests
    {
        private IItemDetailsState<ICruiser> _dismissedState;

        private IItemDetailsManager<ICruiser> _itemsDetailsManager;
        private IItemStateManager _itemStateManager;
        private IItem<ICruiser> _selectedItem;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _itemsDetailsManager = Substitute.For<IItemDetailsManager<ICruiser>>();
			_itemStateManager = Substitute.For<IItemStateManager>();

            _dismissedState = new DismissedState<ICruiser>(_itemsDetailsManager, _itemStateManager);

            _selectedItem = Substitute.For<IItem<ICruiser>>();
        }

        [Test]
        public void SelectItem_ShowsDetails_And_MovesToSelectedState()
        {
            IItemDetailsState<ICruiser> nextState = _dismissedState.SelectItem(_selectedItem);

            _itemsDetailsManager.Received().ShowItemDetails(_selectedItem.Item);
            Assert.IsInstanceOf<SelectedState<ICruiser>>(nextState);
        }

        [Test]
        public void CompareSelectedItem_Throws()
        {
            Assert.Throws<InvalidProgramException>(() => _dismissedState.CompareSelectedItem());
        }

        [Test]
        public void Dismiss_Throws()
        {
            Assert.Throws<InvalidProgramException>(() => _dismissedState.Dismiss());
        }
    }
}
