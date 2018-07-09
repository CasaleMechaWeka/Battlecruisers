using BattleCruisers.AI;
using BattleCruisers.AI.TaskProducers;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.AI
{
    public class ArtificialIntelligenceTests
    {
        private IArtificialIntelligence _ai;
        private ITaskConsumer _taskConsumer;
        private IList<ITaskProducer> _taskProducers;

        [SetUp]
        public void TestSetup()
        {
            _taskConsumer = Substitute.For<ITaskConsumer>();

            _taskProducers = new List<ITaskProducer>()
            {
                Substitute.For<ITaskProducer>(),
                Substitute.For<ITaskProducer>()
            };

            _ai = new ArtificialIntelligence(_taskConsumer, _taskProducers);
        }

        [Test]
        public void DisposeManagedState()
        {
            _ai.DisposeManagedState();

            foreach (ITaskProducer taskproducer in _taskProducers)
            {
                taskproducer.Received().DisposeManagedState();
            }
        }
    }
}
