using System;
using BattleCruisers.AI;
using BattleCruisers.AI.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI
{
    public class TaskConsumerTests
    {
        private ITaskList _tasks;
        private IPrioritisedTask _task1, _task2;

        [SetUp]
        public void SetuUp()
        {
            _task1 = Substitute.For<IPrioritisedTask>();
            _task2 = Substitute.For<IPrioritisedTask>();

            _tasks = Substitute.For<ITaskList>();

            new TaskConsumer(_tasks);
        }

        #region ITaskList.HighestPriorityTaskChanged
        [Test]
        public void NewHighestPriorityTask_StartsNewTask()
        {
            _tasks.HighestPriorityTask.Returns(_task1);
            _tasks.HighestPriorityTaskChanged += Raise.EventWith(_tasks, EventArgs.Empty);

            _task1.Received().Start();
        }

        [Test]
        public void NewHighestPriorityTask_StopsCurrentTask_StartsNewTask()
        {
            NewHighestPriorityTask_StartsNewTask();

            _tasks.HighestPriorityTask.Returns(_task2);
            _tasks.HighestPriorityTaskChanged += Raise.Event();

            _task1.Received().Stop();
            _task2.Received().Start();
        }

        [Test]
        public void SameHighestPriorityTask_DoesNothing()
        {
            // Task started
            _tasks.HighestPriorityTask.Returns(_task1);
            _tasks.HighestPriorityTaskChanged += Raise.EventWith(_tasks, EventArgs.Empty);

            _task1.Received().Start();

            // Task already the current task, so nothing happens
            _task1.ClearReceivedCalls();
            _tasks.HighestPriorityTaskChanged += Raise.EventWith(_tasks, EventArgs.Empty);

            _task1.DidNotReceive().Start();
            _task1.DidNotReceive().Stop();

        }
        #endregion ITaskList.HighestPriorityTaskChanged

        #region ITask.Completed
        [Test]
        public void TaskCompleted_StartsNextTask()
        {
            NewHighestPriorityTask_StartsNewTask();

			_tasks.HighestPriorityTask.Returns(_task2);
            _task1.Completed += Raise.Event();

            // Receives stop even though it has completed
			_task1.Received().Stop();
			_task2.Received().Start();
        }

        [Test]
		public void TaskCompleted_HandlesNoMoreTasks()
		{
			NewHighestPriorityTask_StartsNewTask();

			_tasks.HighestPriorityTask.Returns(default(IPrioritisedTask));
			_task1.Completed += Raise.Event();

			// Receives stop even though it has completed
			_task1.Received().Stop();
            _task2.DidNotReceive().Start();
		}
		#endregion ITask.Completed
	}
}
