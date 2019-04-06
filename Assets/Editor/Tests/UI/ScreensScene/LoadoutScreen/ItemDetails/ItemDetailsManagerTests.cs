using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreen.ItemDetails
{
    public class ItemDetailsManagerTest
    {
        private IItemDetailsManager _itemDetailsManager;
        private IItemDetailsDisplayer<IBuilding> _buildingDetails;
        private IItemDetailsDisplayer<IUnit> _unitDetails;
        private IItemDetailsDisplayer<ICruiser> _cruiserDetails;
        private IBuilding _building;
        private IUnit _unit;
        private ICruiser _cruiser;
        private int _numOfDetailsShownChangeCount, _selectedItemChangeCount, _comparingItemChangeCount;

        [SetUp]
        public void TestSetup()
        {
            _buildingDetails = Substitute.For<IItemDetailsDisplayer<IBuilding>>();
            _unitDetails = Substitute.For<IItemDetailsDisplayer<IUnit>>();
            _cruiserDetails = Substitute.For<IItemDetailsDisplayer<ICruiser>>();

            _itemDetailsManager = new ItemDetailsManager(_buildingDetails, _unitDetails, _cruiserDetails);

            _building = Substitute.For<IBuilding>();
            _unit = Substitute.For<IUnit>();
            _cruiser = Substitute.For<ICruiser>();

            _numOfDetailsShownChangeCount = 0;
            _itemDetailsManager.NumOfDetailsShown.ValueChanged += (sender, e) => _numOfDetailsShownChangeCount++;

            _selectedItemChangeCount = 0;
            _itemDetailsManager.SelectedItem.ValueChanged += (sender, e) => _selectedItemChangeCount++;

            _comparingItemChangeCount = 0;
            _itemDetailsManager.ComparingItem.ValueChanged += (sender, e) => _comparingItemChangeCount++;

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsNull(_itemDetailsManager.SelectedItemFamily);
            Assert.AreEqual(0, _itemDetailsManager.NumOfDetailsShown.Value);
            Assert.IsNull(_itemDetailsManager.SelectedItem.Value);
            Assert.IsNull(_itemDetailsManager.ComparingItem.Value);
        }

        [Test]
        public void ShowDetails_Building()
        {
            _itemDetailsManager.ShowDetails(_building);

            ReceivedHideDetails();
            Assert.AreEqual(ItemFamily.Buildings, _itemDetailsManager.SelectedItemFamily);
            _buildingDetails.Received().SelectItem(_building);
            AssertShowDetailsEvents(_building);
        }

        [Test]
        public void ShowDetails_Unit()
        {
            _itemDetailsManager.ShowDetails(_unit);

            ReceivedHideDetails();
            Assert.AreEqual(ItemFamily.Units, _itemDetailsManager.SelectedItemFamily);
            _unitDetails.Received().SelectItem(_unit);
            AssertShowDetailsEvents(_unit);
        }

        [Test]
        public void ShowDetails_Cruiser()
        {
            _itemDetailsManager.ShowDetails(_cruiser);

            ReceivedHideDetails();
            Assert.AreEqual(ItemFamily.Hulls, _itemDetailsManager.SelectedItemFamily);
            _cruiserDetails.Received().SelectItem(_cruiser);
            AssertShowDetailsEvents(_cruiser);
        }

        [Test]
        public void CompareWithSelectedItem_Building()
        {
            ShowDetails_Building();

            _itemDetailsManager.CompareWithSelectedItem(_building);
            _buildingDetails.Received().CompareWithSelectedItem(_building);
            AssertCompareEvents(_building);
        }

        [Test]
        public void CompareWithSelectedItem_Unit()
        {
            ShowDetails_Unit();

            _itemDetailsManager.CompareWithSelectedItem(_unit);
            _unitDetails.Received().CompareWithSelectedItem(_unit);
            AssertCompareEvents(_unit);
        }

        [Test]
        public void CompareWithSelectedItem_Cruiser()
        {
            ShowDetails_Cruiser();

            _itemDetailsManager.CompareWithSelectedItem(_cruiser);
            _cruiserDetails.Received().CompareWithSelectedItem(_cruiser);
            AssertCompareEvents(_cruiser);
        }

        [Test]
        public void CompareWithSelectedItem_NullSelectedItemFamily_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _itemDetailsManager.CompareWithSelectedItem(_building));
        }

        [Test]
        public void CompareWithSelectedItem_MismatchingSelectedItemFamily_Throws()
        {
            // Selected item family is buildings
            ShowDetails_Building();

            // Compare iwth other family type (units)
            Assert.Throws<UnityAsserts.AssertionException>(() => _itemDetailsManager.CompareWithSelectedItem(_unit));
        }

        [Test]
        public void HideDetails()
        {
            _itemDetailsManager.HideDetails();

            ReceivedHideDetails();
        }

        private void ReceivedHideDetails()
        {
            _buildingDetails.Received().HideDetails();
            _unitDetails.Received().HideDetails();
            _cruiserDetails.Received().HideDetails();
        }

        private void AssertShowDetailsEvents(IComparableItem shownItem)
        {
            Assert.AreEqual(1, _numOfDetailsShownChangeCount);
            Assert.AreEqual(1, _itemDetailsManager.NumOfDetailsShown.Value);
            Assert.AreSame(shownItem, _itemDetailsManager.SelectedItem.Value);
            Assert.AreEqual(1, _selectedItemChangeCount);
        }

        private void AssertCompareEvents(IComparableItem comparedItem)
        {
            Assert.AreEqual(2, _numOfDetailsShownChangeCount);
            Assert.AreEqual(2, _itemDetailsManager.NumOfDetailsShown.Value);
            Assert.AreSame(comparedItem, _itemDetailsManager.ComparingItem.Value);
            Assert.AreEqual(1, _comparingItemChangeCount);
        }
    }
}