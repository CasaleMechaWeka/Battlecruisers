using BattleCruisers.UI.Filters;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Filters
{
    public class CompositeBroadcastingFilterTests
    {
        private CompositeBroadcastingFilter _compositeFilter;
        private BroadcastingFilter _filter1, _filter2;
        private int _matchChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _filter1 = new BroadcastingFilter(false);
            _filter2 = new BroadcastingFilter(true);

            _compositeFilter = new CompositeBroadcastingFilter(true, _filter1, _filter2);

            _matchChangedCount = 0;
            _compositeFilter.PotentialMatchChange += (sender, e) => _matchChangedCount++;
        }

        [Test]
        public void InitialState()
        {
            Assert.IsTrue(_compositeFilter.IsMatch);
            Assert.IsTrue(_filter1.IsMatch);
            Assert.IsTrue(_filter2.IsMatch);
        }

        [Test]
        public void SetIsMatch_SameAsCurrent_DoesNothing()
        {
            Assert.IsTrue(_compositeFilter.IsMatch);

            _compositeFilter.IsMatch = true;

            Assert.IsTrue(_compositeFilter.IsMatch);
            Assert.AreEqual(0, _matchChangedCount);
        }

        [Test]
        public void SetIsMatch_DifferentToCurrent_PropagatesToChildren_EmitsEvent()
        {
            Assert.IsTrue(_compositeFilter.IsMatch);

            _compositeFilter.IsMatch = false;

            Assert.IsFalse(_compositeFilter.IsMatch);
            Assert.IsFalse(_filter1.IsMatch);
            Assert.IsFalse(_filter2.IsMatch);
            Assert.AreEqual(1, _matchChangedCount);
        }
    }
}