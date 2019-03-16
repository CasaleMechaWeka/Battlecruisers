using System.Collections.Generic;
using BattleCruisers.Tutorial;
using BattleCruisers.Tutorial.Steps;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial
{
    public class TutorialStepConsumerTests
    {
        private ITutorialStepConsumer _consumer;
        private Queue<ITutorialStep> _tutorialSteps;
        private ITutorialStep _step1, _step2;

        [SetUp]
        public void SetuUp()
        {
            _step1 = Substitute.For<ITutorialStep>();
            _step2 = Substitute.For<ITutorialStep>();

            _tutorialSteps = new Queue<ITutorialStep>();
            _tutorialSteps.Enqueue(_step1);
            _tutorialSteps.Enqueue(_step2);

            _consumer = new TutorialStepConsumer(_tutorialSteps);
        }

        [Test]
        public void StartConsuming_StartsFirstStep()
        {
            _consumer.StartConsuming();
            _step1.ReceivedWithAnyArgs().Start(null);
        }

        [Test]
        public void StartConsuming_OnFirstStepCompleted_StartsSecondStep()
        {
            _step1.Start(Arg.Invoke());
            _consumer.StartConsuming();
            _step2.ReceivedWithAnyArgs().Start(null);
        }

        [Test]
        public void StartConsuming_OnLastStepCompleted_FiresEvent()
        {
            int eventCounter = 0;
            _consumer.Completed += (sender, e) => eventCounter++;

            _step1.Start(Arg.Invoke());
            _step2.Start(Arg.Invoke());
            _consumer.StartConsuming();

            Assert.AreEqual(1, eventCounter);
        }
    }
}
