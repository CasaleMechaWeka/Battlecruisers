using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Targets.TargetTrackers;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.Movement.Deciders
{
    public class ShipMovementDeciderTests
    {
        private IMovementDecider _shipMovementDecider;

        private IShip _ship;
        private IBroadcastingTargetProvider _blockingEnemyProvider, _blockingFriendlyProvider;
        private ITargetTracker _inRangeTargetTracker, _shipBlockerTargetTracker;
        private ITargetRangeHelper _rangeHelper;
        private ITarget _target, _nullTarget;

        [SetUp]
        public void SetuUp()
        {
            _ship = Substitute.For<IShip>();
            _ship.IsMoving.Returns(false);
            IList<TargetType> attackCapabilities = new List<TargetType>()
            {
                TargetType.Buildings
            };
            ReadOnlyCollection<TargetType> readonlyAttackCapabilities = new ReadOnlyCollection<TargetType>(attackCapabilities);
            _ship.AttackCapabilities.Returns(readonlyAttackCapabilities);

            _blockingEnemyProvider = Substitute.For<IBroadcastingTargetProvider>();
            _blockingFriendlyProvider = Substitute.For<IBroadcastingTargetProvider>();
            _inRangeTargetTracker = Substitute.For<ITargetTracker>();
            _shipBlockerTargetTracker = Substitute.For<ITargetTracker>();
            _rangeHelper = Substitute.For<ITargetRangeHelper>();

            _shipBlockerTargetTracker.ContainsTarget(_ship).Returns(false);

            _target = Substitute.For<ITarget>();
			_nullTarget = null;

            _shipMovementDecider = new ShipMovementDecider(_ship, _blockingEnemyProvider, _blockingFriendlyProvider, _inRangeTargetTracker, _shipBlockerTargetTracker, _rangeHelper);
            _ship.ClearReceivedCalls();
        }

        [Test]
        public void Constructor_ShipStationary_BlockingEnemyTarget_RemainsStationary()
        {
            _ship.DidNotReceive().StartMoving();
            _ship.DidNotReceive().StopMoving();
        }

        #region Events trigger update
        [Test]
        public void BlockingEnemyProvider_TargetChanged_TriggersDecision()
        {
            _blockingEnemyProvider.TargetChanged += Raise.Event();
            AssertDecideMovementWasCalled();
        }

        [Test]
        public void BlockingFriendProvider_TargetChanged_TriggersDecision()
        {
            _blockingFriendlyProvider.TargetChanged += Raise.Event();
            AssertDecideMovementWasCalled();
        }

        [Test]
        public void ShipBlockerTargetTracker_TargetsChanged_TriggersDecision()
        {
            _shipBlockerTargetTracker.TargetsChanged += Raise.Event();
            AssertDecideMovementWasCalled();
        }

        [Test]
        public void HighestPriorityTargetChanged_TriggersDecision()
        {
            _shipMovementDecider.Target = _target;
            AssertDecideMovementWasCalled();
        }

        [Test]
        public void InRangeTargetsChanged_TriggersDecision()
        {
            _inRangeTargetTracker.TargetsChanged += Raise.Event();
            AssertDecideMovementWasCalled();
        }
        #endregion Events trigger update

        #region Decide Movement
        #region Potentially start moving
        [Test]
        public void Stationary_NoBlockingTargets_NoHighPriorityTarget_StartsMoving()
        {
            _ship.IsMoving.Returns(false);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _shipBlockerTargetTracker.ContainsTarget(_ship).Returns(false);
            _shipMovementDecider.Target = null;

            TriggerDecision();

            _ship.Received().StartMoving();
        }

        [Test]
        public void Stationary_NoBlockingTargets_HighPriorityTargetOutOfRange_StartsMoving()
        {
            _ship.IsMoving.Returns(false);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _shipBlockerTargetTracker.ContainsTarget(_ship).Returns(false);
            _shipMovementDecider.Target = _target;
            _rangeHelper.IsTargetInRange(_target).Returns(false);

            TriggerDecision();

            _ship.Received().StartMoving();
        }

        [Test]
        public void Stationary_BlockingEnemy_DoesNothing()
        {
            _ship.IsMoving.Returns(false);
            _blockingEnemyProvider.Target.Returns(_target);

            TriggerDecision();

            _ship.DidNotReceive().StartMoving();
        }

        [Test]
        public void Stationary_BlockingFriend_DoesNothing()
        {
            _ship.IsMoving.Returns(false);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_target);

            TriggerDecision();

            _ship.DidNotReceive().StartMoving();
        }

        [Test]
        public void Stationary_ReachedEnemyCruiser_DoesNothing()
        {
            _ship.IsMoving.Returns(false);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _shipBlockerTargetTracker.ContainsTarget(_ship).Returns(true);

            TriggerDecision();

            _ship.DidNotReceive().StartMoving();
        }

        [Test]
        public void Stationary_NoBlockingTargets_HaveNotReachedEnemyCruiser_HighPriorityTargetInRange_DoesNothing()
        {
            _ship.IsMoving.Returns(false);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _shipBlockerTargetTracker.ContainsTarget(_ship).Returns(false);
            _rangeHelper.IsTargetInRange(_target).Returns(true);
            _shipMovementDecider.Target = _target;

            TriggerDecision();

            _ship.DidNotReceive().StartMoving();
        }
        #endregion Potentially start moving

        #region Potentially stop moving
        [Test]
        public void Moving_BlockingEnemy_StopsMoving()
        {
            _ship.IsMoving.Returns(true);
            _blockingEnemyProvider.Target.Returns(_target);

            TriggerDecision();

            _ship.Received().StopMoving();
        }

        [Test]
        public void Moving_BlockingFriendly_StopsMoving()
        {
            _ship.IsMoving.Returns(true);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_target);

            TriggerDecision();

            _ship.Received().StopMoving();
        }

        [Test]
        public void Moving_NoBlockingTargets_ReachedEnemyCruiser_StopsMoving()
        {
            _ship.IsMoving.Returns(true);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _shipBlockerTargetTracker.ContainsTarget(_ship).Returns(true);

            TriggerDecision();

            _ship.Received().StopMoving();
        }

        [Test]
        public void Moving_NoBlockingTargets_HaveNotReachedEnemyCruiser_HighPriorityTargetInRange_StopsMoving()
        {
            _ship.IsMoving.Returns(true);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _shipBlockerTargetTracker.ContainsTarget(_ship).Returns(false);
            _rangeHelper.IsTargetInRange(_target).Returns(true);
            _shipMovementDecider.Target = _target;

            TriggerDecision();

            _ship.Received().StopMoving();
        }

        [Test]
        public void Moving_NoBlockingTargets_HaveNotReachedEnemyCruiser_HighPriorityTargetOutOfRange_DoesNothing()
        {
            _ship.IsMoving.Returns(true);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _shipBlockerTargetTracker.ContainsTarget(_ship).Returns(false);
            _rangeHelper.IsTargetInRange(_target).Returns(false);
            _shipMovementDecider.Target = _target;

            TriggerDecision();

            _ship.DidNotReceive().StopMoving();
        }
        #endregion Potentially stop moving
        #endregion Decide Movement

        private void AssertDecideMovementWasCalled()
        {
			bool isMoving = _ship.Received().IsMoving;
        }

        private void TriggerDecision()
        {
            _blockingEnemyProvider.TargetChanged += Raise.Event();
        }
    }
}
