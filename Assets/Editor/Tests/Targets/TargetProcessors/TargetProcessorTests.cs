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
        private ITarget _target;

		[SetUp]
		public void TestSetup()
		{
            _targetTracker = Substitute.For<IHighestPriorityTargetTracker>();
			_targetConsumer = Substitute.For<ITargetConsumer>();
			_target = Substitute.For<ITarget>();

			_targetProcessor = new TargetProcessor(_targetTracker);

            _targetProcessor.StartProcessingTargets();
            _targetTracker.Received().StartTrackingTargets();

			UnityAsserts.Assert.raiseExceptions = true;
		}

        // FELIX
	}
}
