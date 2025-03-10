using BattleCruisers.UI;
using BattleCruisers.UI.Filters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI
{
    public class FilterTogglerTests
    {
        private FilterToggler _filterToggler;
        private ITogglable _togglable1, _togglable2;
        private IBroadcastingFilter _shouldBeEnabledFilter;

        [SetUp]
        public void TestSetup()
        {
            _shouldBeEnabledFilter = Substitute.For<IBroadcastingFilter>();
            _shouldBeEnabledFilter.IsMatch.Returns(false);
            _togglable1 = Substitute.For<ITogglable>();
            _togglable2 = Substitute.For<ITogglable>();

            _filterToggler = new FilterToggler(_shouldBeEnabledFilter, _togglable1, _togglable2);
        }

        [Test]
        public void InitialState()
        {
            AssertEnabledWasSet(false);
        }

        [Test]
        public void BroadcastingFilter_PotentialMatchChange_UpdatesTogglable()
        {
            _shouldBeEnabledFilter.IsMatch.Returns(true);
            _shouldBeEnabledFilter.PotentialMatchChange += Raise.Event();

            AssertEnabledWasSet(true);
        }

        private void AssertEnabledWasSet(bool enabled)
        {
            _togglable1.Received().Enabled = enabled;
            _togglable2.Received().Enabled = enabled;
        }
    }
}