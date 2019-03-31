using BattleCruisers.UI.Filters;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Filters
{
    public class BroadcastingFilterTests
    {
        private BroadcastingFilter _filter;
        private int _matchChangeCount;

        [SetUp]
        public void SetuUp()
        {
            _filter = new BroadcastingFilter(isMatch: false);

            _matchChangeCount = 0;
            _filter.PotentialMatchChange += (sender, e) => _matchChangeCount++;
        }

        [Test]
        public void ChangeIsMatch_DifferentToCurrent_TriggersEvent()
        {
            Assert.IsFalse(_filter.IsMatch);

            _filter.IsMatch = true;

            Assert.IsTrue(_filter.IsMatch);
            Assert.AreEqual(1, _matchChangeCount);
        }

        [Test]
        public void ChangeIsMatch_SameAsCurrent_DoesNothing()
        {
            Assert.IsFalse(_filter.IsMatch);

            _filter.IsMatch = false;

            Assert.IsFalse(_filter.IsMatch);
            Assert.AreEqual(0, _matchChangeCount);
        }
    }
}
