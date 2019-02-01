using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreenNEW.ItemDetails
{
    public class ItemDetailsDisplayerTests
    {
        private IItemDetailsDisplayer<IBuilding> _displayer;
        private IComparableItemDetails<IBuilding> _leftDetails, _rightDetails;
        private IBuilding _building1, _building2;

        [SetUp]
        public void TestSetup()
        {
            _leftDetails = Substitute.For<IComparableItemDetails<IBuilding>>();
            _rightDetails = Substitute.For<IComparableItemDetails<IBuilding>>();

            _displayer = new ItemDetailsDisplayer<IBuilding>(_leftDetails, _rightDetails);

            _building1 = Substitute.For<IBuilding>();
            _building2 = Substitute.For<IBuilding>();

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void SelectItem()
        {
            _displayer.SelectItem(_building1);

            ReceivedHideDetails();
            _leftDetails.Received().ShowItemDetails(_building1);
            Assert.AreSame(_building1, _displayer.SelectedItem.Value);
        }

        [Test]
        public void CompareWithSelectedItem()
        {
            SelectItem();

            _displayer.CompareWithSelectedItem(_building2);

            _leftDetails.Received().ShowItemDetails(_building1, _building2);
            _rightDetails.Received().ShowItemDetails(_building2, _building1);
        }

        [Test]
        public void CompareWithSelectedItem_NoSelectedItem_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _displayer.CompareWithSelectedItem(_building1));
        }

        [Test]
        public void HideDetails()
        {
            _displayer.HideDetails();
            ReceivedHideDetails();
            Assert.IsNull(_displayer.SelectedItem.Value);
        }

        private void ReceivedHideDetails()
        {
            _leftDetails.Received().Hide();
            _rightDetails.Received().Hide();
        }
    }
}