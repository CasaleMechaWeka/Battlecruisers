using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils.Properties;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.ScreensScene.LoadoutScreenNEW.Comparisons
{
    public class ComparisonStateTrackerTests
    {
        private IComparisonStateTracker _stateTracker;
        private ISettableBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;
        private IItemDetailsManager _itemDetailsManager;
        private int _stateChangedCounter;

        [SetUp]
        public void TestSetup()
        {
            _itemFamilyToCompare = Substitute.For<ISettableBroadcastingProperty<ItemFamily?>>();
            _itemFamilyToCompare.Value.Returns((ItemFamily?)null);

            _itemDetailsManager = Substitute.For<IItemDetailsManager>();
            _itemDetailsManager.NumOfDetailsShown.Value.Returns(0);

            _stateTracker = new ComparisonStateTracker(_itemFamilyToCompare, _itemDetailsManager);

            _stateChangedCounter = 0;
            _stateTracker.State.ValueChanged += (sender, e) => _stateChangedCounter++;
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(ComparisonState.NotComparing, _stateTracker.State.Value);
        }

        [Test]
        public void ItemFamilyToCompareChanged_EvaluatesState()
        {
            _itemFamilyToCompare.Value.Returns(ItemFamily.Hulls);
            _itemFamilyToCompare.ValueChanged += Raise.Event();

            Assert.AreEqual(1, _stateChangedCounter);
            Assert.AreEqual(ComparisonState.ReadyToCompare, _stateTracker.State.Value);
        }

        [Test]
        public void NumOfDetailsShownChanged_EvaluatesState()
        {
            _itemDetailsManager.NumOfDetailsShown.Value.Returns(2);
            _itemDetailsManager.NumOfDetailsShown.ValueChanged += Raise.Event();

            Assert.AreEqual(1, _stateChangedCounter);
            Assert.AreEqual(ComparisonState.Comparing, _stateTracker.State.Value);
        }
    }
}