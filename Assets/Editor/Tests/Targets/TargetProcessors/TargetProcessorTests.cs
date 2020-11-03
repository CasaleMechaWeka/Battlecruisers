using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.TargetProcessors
{
    public class TargetProcessorTests
    {
        private ITargetProcessor _targetProcessor;
        private IRankedTargetTracker _targetTracker;
        private ITargetConsumer _targetConsumer;
        private RankedTarget _target1, _target2;

        [SetUp]
        public void TestSetup()
        {
            _targetTracker = Substitute.For<IRankedTargetTracker>();
            _targetConsumer = Substitute.For<ITargetConsumer>();
            _target1 = new RankedTarget(Substitute.For<ITarget>(), 12);
            _target2 = new RankedTarget(Substitute.For<ITarget>(), 13);

            _targetProcessor = new TargetProcessor(_targetTracker);

            _targetTracker.HighestPriorityTarget.Returns(_target1);
        }

        #region AddTargetConsumer
        [Test]
        public void AddTargetConsumer_AssignsTarget()
        {
            _targetProcessor.AddTargetConsumer(_targetConsumer);
            _targetConsumer.Received().Target = _target1.Target;
        }

        [Test]
        public void AddTargetConsumer_DuplicateConsumer_Throws()
        {
            _targetProcessor.AddTargetConsumer(_targetConsumer);
            Assert.Throws<UnityAsserts.AssertionException>(() => _targetProcessor.AddTargetConsumer(_targetConsumer));
        }
        #endregion AddTargetConsumer

        #region RemoveTargetConsumer
        [Test]
        public void RemoveTargetConsumer_RemovesConsumer()
        {
            _targetProcessor.AddTargetConsumer(_targetConsumer);
            _targetProcessor.RemoveTargetConsumer(_targetConsumer);

            _targetConsumer.ClearReceivedCalls();

            InvokeTargetChanged();

            _targetConsumer.DidNotReceiveWithAnyArgs().Target = null;
        }

        [Test]
        public void RemoveTargetConsumer_NotPreviouslyAdded_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _targetProcessor.RemoveTargetConsumer(_targetConsumer));
        }
        #endregion RemoveTargetConsumer

        [Test]
        public void HighestPriorityTargetChanged_UpdatesConsumers()
        {
            _targetProcessor.AddTargetConsumer(_targetConsumer);
            _targetConsumer.Received().Target = _target1.Target;

            _targetTracker.HighestPriorityTarget.Returns(_target2);
            InvokeTargetChanged();

            _targetConsumer.Received().Target = _target2.Target;
        }

        private void InvokeTargetChanged()
        {
            _targetTracker.HighestPriorityTargetChanged += Raise.Event();
        }
    }
}
