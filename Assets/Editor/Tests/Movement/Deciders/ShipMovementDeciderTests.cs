using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Targets;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProcessors.Ranking;
using BattleCruisers.Targets.TargetProviders;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Movement.Deciders
{
    public class ShipMovementDeciderTests
    {
        private IMovementDecider _shipMovementDecider;

        private IShip _ship;
        private ITargetsFactory _targetsFactory;
        private ITargetRangeHelper _rangeHelper;
        private IBroadCastingTargetProvider _blockingEnemyProvider, _blockingFriendlyProvider;
        private IHighestPriorityTargetProvider _highPriorityTarget;
        private ITargetProvider _highestPriorityTargetProvider;
        private ITargetConsumer _highestPriorityTargetConsumer;
        private ITarget _target, _nullTarget;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _ship = Substitute.For<IShip>();
            _ship.IsMoving.Returns(false);
            IList<TargetType> attackCapabilities = new List<TargetType>()
            {
                TargetType.Buildings
            };
            _ship.AttackCapabilities.Returns(attackCapabilities);

            ITargetDetector enemyDetector = Substitute.For<ITargetDetector>();
            ITargetDetector friendDetector = Substitute.For<ITargetDetector>();
			
            _targetsFactory = Substitute.For<ITargetsFactory>();

            _rangeHelper = Substitute.For<ITargetRangeHelper>();
            _targetsFactory.CreateShipRangeHelper(_ship).Returns(_rangeHelper);

            _blockingEnemyProvider = Substitute.For<IBroadCastingTargetProvider>();
            _targetsFactory.CreateShipBlockingEnemyProvider(enemyDetector, _ship).Returns(_blockingEnemyProvider);

            _blockingFriendlyProvider = Substitute.For<IBroadCastingTargetProvider>();
            _targetsFactory.CreateShipBlockingFriendlyProvider(friendDetector, _ship).Returns(_blockingFriendlyProvider);

            ITargetRanker shipRanker = Substitute.For<ITargetRanker>();
            _targetsFactory.CreateShipTargetRanker().Returns(shipRanker);

            ITargetFilter attackingTargetFilter = Substitute.For<ITargetFilter>();
            _targetsFactory.CreateTargetFilter(default(Faction), null).ReturnsForAnyArgs(attackingTargetFilter);

            _highPriorityTarget = Substitute.For<IHighestPriorityTargetProvider>();
            _targetsFactory.CreateHighestPriorityTargetProvider(shipRanker, attackingTargetFilter, _ship).Returns(_highPriorityTarget);

            _highestPriorityTargetConsumer = _highPriorityTarget;
            _highestPriorityTargetProvider = _highPriorityTarget;

            _target = Substitute.For<ITarget>();
            _blockingEnemyProvider.Target.Returns(_target);
			_nullTarget = null;

            _shipMovementDecider = new ShipMovementDecider(_ship, _targetsFactory, enemyDetector, friendDetector);

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
        public void HighestPriorityTargetProvider_TargetChanged_TriggersDecision()
        {
            _highPriorityTarget.TargetChanged += Raise.Event();
            AssertDecideMovementWasCalled();
        }

        [Test]
        public void HighestPriorityTargetProvider_NewInRangeTarget_TriggersDecision()
        {
            _highPriorityTarget.NewInRangeTarget += Raise.Event();
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
            _highestPriorityTargetProvider.Target.Returns(_nullTarget);

            TriggerDecision();

            _ship.Received().StartMoving();
        }

        [Test]
        public void Stationary_NoBlockingTargets_HighPriorityTargetOutOfRange_StartsMoving()
        {
            _ship.IsMoving.Returns(false);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _highestPriorityTargetProvider.Target.Returns(_target);
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
        public void Stationary_NoBlockingTargets_HighPriorityTargetInRange_DoesNothing()
        {
            _ship.IsMoving.Returns(false);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _highestPriorityTargetProvider.Target.Returns(_target);
            _rangeHelper.IsTargetInRange(_target).Returns(true);

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
        public void Moving_NoBlockingTargets_HighPriorityTargetInRange_StopsMoving()
        {
            _ship.IsMoving.Returns(true);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _highestPriorityTargetProvider.Target.Returns(_target);
            _rangeHelper.IsTargetInRange(_target).Returns(true);

            TriggerDecision();

            _ship.Received().StopMoving();
        }

        [Test]
        public void Moving_NoBlockingTargets_HighPriorityTargetOutOfRange_DoesNothing()
        {
            _ship.IsMoving.Returns(true);
            _blockingEnemyProvider.Target.Returns(_nullTarget);
            _blockingFriendlyProvider.Target.Returns(_nullTarget);
            _highestPriorityTargetProvider.Target.Returns(_target);
            _rangeHelper.IsTargetInRange(_target).Returns(false);

            TriggerDecision();

            _ship.DidNotReceive().StopMoving();
        }
        #endregion Potentially stop moving
        #endregion Decide Movement

        [Test]
        public void SetDeciderTarget_UpdatesHighestPriorityTargetProviderTarget()
        {
            _highPriorityTarget.ClearReceivedCalls();

            _shipMovementDecider.Target = _target;

            _highestPriorityTargetConsumer.Received().Target = _target;
        }

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
