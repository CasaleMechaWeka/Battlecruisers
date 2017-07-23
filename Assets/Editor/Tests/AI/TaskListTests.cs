using System;
using BattleCruisers.AI;
using BattleCruisers.AI.Tasks;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI
{
	public class TaskListTests
	{
        private ITaskList _taskList;

        private ITask _normalTask1, _normalTask2, _highTask1, _highTask2;
        private int _numOfEventCalls;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;

            _taskList = new TaskList();

            _normalTask1 = CreateMockTask(TaskPriority.Normal);
            _normalTask2 = CreateMockTask(TaskPriority.Normal);
            _highTask1 = CreateMockTask(TaskPriority.High);
            _highTask2 = CreateMockTask(TaskPriority.High);
			
			_numOfEventCalls = 0;
            _taskList.HighestPriorityTaskChanged += _taskList_HighestPriorityTaskChanged;
        }

        private ITask CreateMockTask(TaskPriority priority)
        {
			ITask task = Substitute.For<ITask>();
            task.Priority.Returns(priority);
            return task;
        }

		[Test]
		public void Add_SingleTask()
		{
            Assert.IsTrue(_taskList.IsEmpty);

            _taskList.Add(_normalTask1);

            Assert.IsFalse(_taskList.IsEmpty);
            Assert.AreEqual(1, _numOfEventCalls);
            Assert.AreSame(_normalTask1, _taskList.HighestPriorityTask);
		}

        [Test]
        public void Add_ManyTasks()
        {
            Assert.IsTrue(_taskList.IsEmpty);

            _taskList.Add(_normalTask1);    // N1
			Assert.IsFalse(_taskList.IsEmpty);
			Assert.AreEqual(1, _numOfEventCalls);
			Assert.AreSame(_normalTask1, _taskList.HighestPriorityTask);

			_taskList.Add(_highTask1);      // N1, H1
			Assert.AreEqual(2, _numOfEventCalls);
            Assert.AreSame(_highTask1, _taskList.HighestPriorityTask);

            _taskList.Add(_normalTask2);    // N2, N1, H1
			Assert.AreEqual(2, _numOfEventCalls);
			Assert.AreSame(_highTask1, _taskList.HighestPriorityTask);

            _taskList.Add(_highTask2);      // N2, N1, H2, H1
			Assert.AreEqual(2, _numOfEventCalls);
			Assert.AreSame(_highTask1, _taskList.HighestPriorityTask);
        }

        [Test]
        public void Add_Duplicate_Throws()
        {
            _taskList.Add(_normalTask1);
			Assert.Throws<UnityAsserts.AssertionException>(() => _taskList.Add(_normalTask1));
		}

        [Test]
        public void Remove_SingleTask()
        {
            Add_SingleTask();

            ResetEventCalls();
            _taskList.Remove(_normalTask1);
            Assert.IsTrue(_taskList.IsEmpty);
            Assert.AreEqual(1, _numOfEventCalls);
        }
		
        [Test]
		public void Remove_ManyTask()
        {
            Add_ManyTasks();  // N2, N1, H2, H1

			ResetEventCalls();

			Assert.AreSame(_highTask1, _taskList.HighestPriorityTask);

			_taskList.Remove(_taskList.HighestPriorityTask);  // N2, N1, H2
			Assert.AreSame(_highTask2, _taskList.HighestPriorityTask);
			Assert.AreEqual(1, _numOfEventCalls);

			_taskList.Remove(_taskList.HighestPriorityTask);  // N2, N1
            Assert.AreSame(_normalTask1, _taskList.HighestPriorityTask);
			Assert.AreEqual(2, _numOfEventCalls);

			_taskList.Remove(_taskList.HighestPriorityTask);  // N2
	        Assert.AreSame(_normalTask2, _taskList.HighestPriorityTask);
			Assert.AreEqual(3, _numOfEventCalls);

            _taskList.Remove(_taskList.HighestPriorityTask);  // (empty)
            Assert.IsNull(_taskList.HighestPriorityTask);
			Assert.AreEqual(4, _numOfEventCalls);
		}

		[Test]
		public void Remove_NonExistentTask_Throws()
		{
            Assert.Throws<UnityAsserts.AssertionException>(() => _taskList.Remove(_normalTask1));
		}

        private void _taskList_HighestPriorityTaskChanged(object sender, EventArgs e)
        {
            Assert.AreSame(_taskList, sender);
            _numOfEventCalls++;
        }

        private void ResetEventCalls()
        {
            _numOfEventCalls = 0;
        }
    }
}
