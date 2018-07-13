using BattleCruisers.AI.Tasks;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.AI.Tasks
{
    public class DeferredPrioritisedTaskTests
    {
        private IPrioritisedTask _deferredTask, _baseTask;
        private IDeferrer _deferrer;

        [SetUp]
        public void TestSetup()
        {
            _baseTask = Substitute.For<IPrioritisedTask>();
            _deferrer = Substitute.For<IDeferrer>();

            _deferredTask = new DeferredPrioritisedTask(_baseTask, _deferrer);

            _deferrer
                .WhenForAnyArgs(deferrer => deferrer.Defer(null))
                .Do(callInfo =>
                {
                    Assert.IsTrue(callInfo.Args().Length == 1);
                    Action actionToDefer = callInfo.Args()[0] as Action;
                    Assert.IsNotNull(actionToDefer);
                    actionToDefer.Invoke();
                });
        }


        [Test]
        public void Start_DefersStart()
        {
            _deferredTask.Start();

            _deferrer.ReceivedWithAnyArgs().Defer(null);
            _baseTask.Received().Start();
        }

        [Test]
        public void Stop_DefersStop()
        {
            _deferredTask.Stop();

            _deferrer.ReceivedWithAnyArgs().Defer(null);
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
