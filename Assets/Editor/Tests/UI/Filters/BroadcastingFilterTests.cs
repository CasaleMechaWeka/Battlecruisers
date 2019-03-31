using BattleCruisers.UI.Filters;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Filters
{
    public class BroadcastingFilterTests
    {
        private BroadcastingFilter _filter;

        [SetUp]
        public void SetuUp()
        {
            _filter = new BroadcastingFilter(isMatch: false);
        }

        [Test]
        public void ChangeIsMatch_TriggersEvent()
        {
            Assert.IsFalse(_filter.IsMatch);

            int eventCounter = 0;
            _filter.PotentialMatchChange += (sender, e) => eventCounter++;

            _filter.IsMatch = true;

            Assert.IsTrue(_filter.IsMatch);
            Assert.AreEqual(1, eventCounter);
        }
    }
}
