using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreen.States
{
    public abstract class ItemDetailsStateTests
    {
        protected IItemDetailsState<ICruiser> _itemState;

        protected IItemDetailsManager<ICruiser> _itemsDetailsManager;
        protected IItemStateManager _itemStateManager;
        protected IItem<ICruiser> _selectedItem, _itemToCompare;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _itemsDetailsManager = Substitute.For<IItemDetailsManager<ICruiser>>();
            _itemStateManager = Substitute.For<IItemStateManager>();
            _selectedItem = Substitute.For<IItem<ICruiser>>();
			_itemToCompare = Substitute.For<IItem<ICruiser>>();

            _itemState = CreateItemState();
        }

        protected abstract IItemDetailsState<ICruiser> CreateItemState();
    }
}
