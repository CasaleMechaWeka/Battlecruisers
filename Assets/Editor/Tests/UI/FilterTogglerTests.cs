using BattleCruisers.UI;
using BattleCruisers.UI.Filters;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI
{
    public class FilterTogglerTests
    {
        private FilterToggler _filterToggler;
        private ITogglable _togglable;
        private IBroadcastingFilter _shouldBeEnabledFilter;

        [SetUp]
        public void TestSetup()
        {
            _togglable = Substitute.For<ITogglable>();
            _shouldBeEnabledFilter = Substitute.For<IBroadcastingFilter>();
            _shouldBeEnabledFilter.IsMatch.Returns(false);

            _filterToggler = new FilterToggler(_togglable, _shouldBeEnabledFilter);
        }

        [Test]
        public void InitialState()
        {
            _togglable.Received().Enabled = false;
        }

        [Test]
        public void BroadcastingFilter_PotentialMatchChange_UpdatesTogglable()
        {
            _shouldBeEnabledFilter.IsMatch.Returns(true);
            _shouldBeEnabledFilter.PotentialMatchChange += Raise.Event();

            _togglable.Received().Enabled = true;
        }
    }
}