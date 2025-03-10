using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProviders;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Targets.TargetProviders
{
    public class ShipBlockingFriendlyProviderTests
    {
        private IBroadcastingTargetProvider _targetProvider;
        private ITargetFinder _friendFinder;
        private ITargetFilter _isInFrontFilter;
        private ITarget _target, _target2;

        [SetUp]
        public void SetuUp()
        {
            _friendFinder = Substitute.For<ITargetFinder>();
            _isInFrontFilter = Substitute.For<ITargetFilter>();
            _target = Substitute.For<ITarget>();
            _target2 = Substitute.For<ITarget>();

            ITargetFactoriesProvider targetFactories = Substitute.For<ITargetFactoriesProvider>();
            ITargetDetector friendDetector = Substitute.For<ITargetDetector>();
            ITargetFilter friendFilter = Substitute.For<ITargetFilter>();
            IUnit parentUnit = Substitute.For<IUnit>();

            targetFactories.FilterFactory.CreateTargetInFrontFilter(parentUnit).Returns(_isInFrontFilter);
            targetFactories.FilterFactory.CreateTargetFilter(default, targetTypes: null).ReturnsForAnyArgs(friendFilter);
            targetFactories.FinderFactory.CreateRangedTargetFinder(friendDetector, friendFilter).Returns(_friendFinder);

            _targetProvider = new ShipBlockingFriendlyProvider(targetFactories, friendDetector, parentUnit);
        }

        [Test]
        public void Constructor_TargetIsNull()
        {
            Assert.IsNull(_targetProvider.Target);
        }

        #region TargetFound
        [Test]
        public void TargetFound_NotInFront_DoesNothing()
        {
            _isInFrontFilter.IsMatch(_target).Returns(false);
            _friendFinder.TargetFound += Raise.EventWith(_friendFinder, new TargetEventArgs(_target));
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void TargetFound_InFront_Destroyed_DoesNothing()
        {
            _isInFrontFilter.IsMatch(_target).Returns(true);
            _target.IsDestroyed.Returns(true);
            _friendFinder.TargetFound += Raise.EventWith(_friendFinder, new TargetEventArgs(_target));
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void TargetFound_IsInFront_AndNotDestroyed_UpdatesTarget()
        {
            FindFriend(_target);
            Assert.AreSame(_target, _targetProvider.Target);
        }
        #endregion TargetFound

        #region TargetLost
        [Test]
        public void TargetLost_NotInFront_DoesNothing()
        {
            _isInFrontFilter.IsMatch(_target).Returns(false);
            _friendFinder.TargetLost += Raise.EventWith(_friendFinder, new TargetEventArgs(_target));
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void TargetLost_NoCurrentTarget_DoesNothing()
        {
            _friendFinder.TargetLost += Raise.EventWith(_friendFinder, new TargetEventArgs(_target));
            Assert.IsNull(_targetProvider.Target);
        }

        [Test]
        public void TargetLost_IsInFront_IsCurrentTarget_UpdatesTarget()
        {
            FindFriend(_target);

            _friendFinder.TargetLost += Raise.EventWith(_friendFinder, new TargetEventArgs(_target));
            Assert.IsNull(_targetProvider.Target);
        }
        #endregion TargetLost

        private void FindFriend(ITarget friend)
        {
            _isInFrontFilter.IsMatch(friend).Returns(true);
            _friendFinder.TargetFound += Raise.EventWith(_friendFinder, new TargetEventArgs(friend));
        }
    }
}
