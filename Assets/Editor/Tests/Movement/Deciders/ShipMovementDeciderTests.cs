using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units.Ships;
using BattleCruisers.Movement.Deciders;
using BattleCruisers.Targets;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetFinders;
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
        private ITarget _target;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _ship = Substitute.For<IShip>();
            _ship.IsMoving.Returns(false);

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

            _highPriorityTarget = Substitute.For<IHighestPriorityTargetProvider>();
            _targetsFactory.CreateHighestPriorityTargetProvider(shipRanker, _ship).Returns(_highPriorityTarget);

            _target = Substitute.For<ITarget>();
            _blockingEnemyProvider.Target.Returns(_target);

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
        public void BlockingEnemyProvider_TargetChanged_TriggersUpdate()
        {
            _blockingEnemyProvider.TargetChanged += Raise.Event();
            AssertUpdateVelocityWasCalled();
        }

        [Test]
        public void BlockingFriendProvider_TargetChanged_TriggersUpdate()
        {
            _blockingFriendlyProvider.TargetChanged += Raise.Event();
            AssertUpdateVelocityWasCalled();
        }

        [Test]
        public void HighestPriorityTargetProvider_TargetChanged_TriggersUpdate()
        {
            _highPriorityTarget.TargetChanged += Raise.Event();
            AssertUpdateVelocityWasCalled();
        }

        [Test]
        public void HighestPriorityTargetProvider_NewInRangeTarget_TriggersUpdate()
        {
            _highPriorityTarget.NewInRangeTarget += Raise.Event();
            AssertUpdateVelocityWasCalled();
        }
        #endregion Events trigger update

        private void AssertUpdateVelocityWasCalled()
        {
			bool isMoving = _ship.Received().IsMoving;
        }
    }
}
