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

			_tasks.HighestPriorityTask.Returns(default(ITask));
			_task1.Completed += Raise.Event();

			// Receives stop even though it has completed
			_task1.Received().Stop();
            _task2.DidNotReceive().Start();
		}
		#endregion ITask.Completed
	}
}
