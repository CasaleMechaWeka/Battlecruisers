using BattleCruisers.AI.Tasks;
using BattleCruisers.Tests.Mock;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Tasks
{
    public class DeferredPrioritisedTaskTests
    {
        private IPrioritisedTask _deferredTask, _baseTask;
        private IDeferrer _deferrer;
        private IDelayProvider _delayProvider;

        [SetUp]
        public void TestSetup()
        {
            _baseTask = Substitute.For<IPrioritisedTask>();
            _deferrer = SubstituteFactory.CreateDeferrer();
            _delayProvider = Substitute.For<IDelayProvider>();
            _delayProvider.DelayInS.Returns(1);

            _deferredTask = new DeferredPrioritisedTask(_baseTask, _deferrer, _delayProvider);
        }

        [Test]
        public void Start_DefersStart()
        {
            _deferredTask.Start();

            _deferrer.ReceivedWithAnyArgs().Defer(null, default);
            _baseTask.Received().Start();
        }

        [Test]
        public void Stop_DefersStop()
        {
            _deferredTask.Stop();

            _deferrer.ReceivedWithAnyArgs().Defer(null, default);
            _baseTask.Received().Stop();
        }

        [Test]
        public void Priority_ForwardsPriority()
        {
            _baseTask.Priority.Returns(TaskPriority.Normal);
            Assert.AreEqual(_baseTask.Priority, _deferredTask.Priority);
        }

        [Test]
        public void CompletedEvent()
        {
            int completedCount = 0;

            _deferredTask.Completed += (sender, e) => completedCount++;

            _baseTask.Completed += Raise.Event();

            Assert.AreEqual(1, completedCount);
        }
    }
}
