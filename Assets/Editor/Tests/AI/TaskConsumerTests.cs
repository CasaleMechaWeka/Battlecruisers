using System;
using BattleCruisers.AI;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI
{
    public class TaskConsumerTests
    {
        private ITaskConsumer _taskConsumer;

        private ITaskList _tasks;
        private ITask _task1, _task2;

        [SetUp]
        public void SetuUp()
        {
            _task1 = Substitute.For<ITask>();
            _task2 = Substitute.For<ITask>();

            _tasks = Substitute.For<ITaskList>();

            _taskConsumer = new TaskConsumer(_tasks);
        }

        [Test]
        public void NewHighestPriorityEvent_StartsNewEvent()
        {
            _tasks.HighestPriorityTask.Returns(_task1);
            _tasks.HighestPriorityTaskChanged += Raise.EventWith(_tasks, EventArgs.Empty);

			_task1.Received().Start();
        }
	}
}
