using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProviders;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

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
            UnityAsserts.Assert.raiseExceptions = true;

            _friendFinder = Substitute.For<ITargetFinder>();
            _isInFrontFilter = Substitute.For<ITargetFilter>();
            _target = Substitute.For<ITarget>();
            _target2 = Substitute.For<ITarget>();

            ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();
            ITargetDetector friendDetector = Substitute.For<ITargetDetector>();
            ITargetFilter friendFilter = Substitute.For<ITargetFilter>();
            IUnit parentUnit = Substitute.For<IUnit>();

            targetsFactory.CreateTargetInFrontFilter(parentUnit).Returns(_isInFrontFilter);
            targetsFactory.CreateTargetFilter(default(Faction), targetTypes: null).ReturnsForAnyArgs(friendFilter);
            targetsFactory.CreateRangedTargetFinder(friendDetector, friendFilter).Returns(_friendFinder);

            _targetProvider = new ShipBlockingFriendlyProvider(targetsFactory, friendDetector, parentUnit);
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
        public void TargetLost_InFront_NoCurrentTarget_Throws()
        {
            _isInFrontFilter.IsMatch(_target).Returns(true);
            Assert.Throws<UnityAsserts.AssertionException>(() =>
            {
                _friendFinder.TargetLost += Raise.EventWith(_friendFinder, new TargetEventArgs(_target));
            });
        }

        [Test]
        public void TargetLost_IsInFront_NotCurrentTarget_DoesNothing()
        {
            FindFriend(_target);

            _isInFrontFilter.IsMatch(_target2).Returns(false);
            _friendFinder.TargetLost += Raise.EventWith(_friendFinder, new TargetEventArgs(_target2));
            Assert.AreSame(_target, _targetProvider.Target);
        }

        [Test]
        public void TargetLost_IsInFront_IsCurrentTarget_UpdatesTarget()
        {
            FindFriend(_target);

            _isInFrontFilter.IsMatch(_target).Returns(true);
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
