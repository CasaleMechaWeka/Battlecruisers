using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreen.Items
{
    public class LoadoutItemColourControllerTests
    {
        private LoadoutItemColourController _colourController;
        private IItemDetailsManager _itemDetails;
        private IList<IItemButton> _itemButtons;
        private IItemButton _button1, _button2;
        private IComparableItem _item1, _item2, _itemWithoutButton;

        [SetUp]
        public void TestSetup()
        {
            _itemDetails = Substitute.For<IItemDetailsManager>();

            _button1 = Substitute.For<IItemButton>();
            _item1 = Substitute.For<IComparableItem>();
            _button1.Item.Returns(_item1);

            _button2 = Substitute.For<IItemButton>();
            _item2 = Substitute.For<IComparableItem>();
            _button2.Item.Returns(_item2);

            _itemButtons = new List<IItemButton>()
            {
                _button1,
                _button2
            };

            _itemWithoutButton = Substitute.For<IComparableItem>();

            _colourController = new LoadoutItemColourController(_itemDetails, _itemButtons);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void SelectedItemChanged_HighlightsNewItem()
        {
            SelectItem(_item1);
            _button1.Received().Color = ButtonColour.Selected;
        }

        [Test]
        public void SelectedItemChanged_UnhighlightsCurrentItem()
        {
            SelectItem(_item1);
            SelectItem(_item2);
            _button1.Received().Color = ButtonColour.Default;
            _button2.Received().Color = ButtonColour.Selected;
        }

        [Test]
        public void SelectedItemChanged_NewIsNull_UnhighlightsCurrentItem()
        {
            SelectItem(_item1);
            SelectItem(null);
            _button1.Received().Color = ButtonColour.Default;
        }

        [Test]
        public void SelectedItemChanged_UnknownItem_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => SelectItem(_itemWithoutButton));
        }

        [Test]
        public void ComparingItemChanged_HighlightsNewItem()
        {
            CompareItem(_item1);
            _button1.Received().Color = ButtonColour.Selected;
        }

        [Test]
        public void ComparingItemChanged_UnhighlightsCurrentItem()
        {
            CompareItem(_item1);
            CompareItem(_item2);
            _button1.Received().Color = ButtonColour.Default;
            _button2.Received().Color = ButtonColour.Selected;
        }

        [Test]
        public void ComparingItemChanged_NewIsNull_UnhighlightsCurrentItem()
        {
            CompareItem(_item1);
            CompareItem(null);
            _button1.Received().Color = ButtonColour.Default;
        }

        [Test]
        public void ComparingItemChanged_UnknownItem_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => CompareItem(_itemWithoutButton));
        }

        private void SelectItem(IComparableItem itemToSelect)
        {
            _itemDetails.SelectedItem.Value.Returns(itemToSelect);
            _itemDetails.SelectedItem.ValueChanged += Raise.Event();
        }

        private void CompareItem(IComparableItem itemToCompare)
        {
            _itemDetails.ComparingItem.Value.Returns(itemToCompare);
            _itemDetails.ComparingItem.ValueChanged += Raise.Event();
        }
    }
}