using BattleCruisers.Buildables;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetProcessors;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Targets.TargetProcessors
{
    public class TargetProcessorTests
	{
		private ITargetProcessor _targetProcessor;
        private IHighestPriorityTargetTracker _targetTracker;
		private ITargetConsumer _targetConsumer;
        private ITarget _target1, _target2;

		[SetUp]
		public void TestSetup()
		{
            _targetTracker = Substitute.For<IHighestPriorityTargetTracker>();
			_targetConsumer = Substitute.For<ITargetConsumer>();
			_target1 = Substitute.For<ITarget>();
			_target2 = Substitute.For<ITarget>();

			_targetProcessor = new TargetProcessor(_targetTracker);

            _targetProcessor.StartProcessingTargets();
            _targetTracker.Received().StartTrackingTargets();

            _targetTracker.HighestPriorityTarget.Returns(_target1);

			UnityAsserts.Assert.raiseExceptions = true;
		}

        #region AddTargetConsumer
        [Test]
        public void AddTargetConsumer_AssignsTarget()
        {
            _targetProcessor.AddTargetConsumer(_targetConsumer);
            _targetConsumer.Received().Target = _target1;
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
            _targetConsumer.Received().Target = _target1;

            _targetTracker.HighestPriorityTarget.Returns(_target2);
            InvokeTargetChanged();

            _targetConsumer.Received().Target = _target2;
        }

        private void InvokeTargetChanged()
        {
            _targetTracker.HighestPriorityTargetChanged += Raise.Event();
        }
    }
}
