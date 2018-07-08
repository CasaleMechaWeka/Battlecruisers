using System;
using BattleCruisers.AI.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Tasks
{
    public class PrioritisedTaskTests
    {
        private IPrioritisedTask _taskController;
        private IInternalTask _task;
        private int _numOfCompletedEvents;

        [SetUp]
        public void SetuUp()
        {
            _task = Substitute.For<IInternalTask>();
            _taskController = new PrioritisedTask(TaskPriority.High, _task);
            _taskController.Completed += _task_Completed;

            _numOfCompletedEvents = 0;
        }

        [Test]
        public void Completed_EmitsCompletedEvent()
        {
            _taskController.Start();
            _task.Completed += Raise.Event();
            Assert.AreEqual(1, _numOfCompletedEvents);
        }

        #region ITask.Start()
        [Test]
        public void Start_WhileNotStarted_StartsTask()
        {
            _taskController.Start();
            _task.Received().Start();
        }

		[Test]
		public void Start_WhileInProgress_DoesNothing()
		{
            _taskController.Start();

            _task.ClearReceivedCalls();
            _taskController.Start();
            _task.DidNotReceive().Start();
		}
		
		[Test]
		public void Start_WhileStopped_ResumesTask()
		{
			_taskController.Start();
			_taskController.Stop();
			
			_taskController.Start();
			_task.Received().Resume();
		}

        [Test]
        public void Start_WhileCompleted_EmitsCompletedEvent()
        {
            _taskController.Start();

            _task.Completed += Raise.Event();
			Assert.AreEqual(1, _numOfCompletedEvents);

			_task.ClearReceivedCalls();
            _taskController.Start();
            _task.DidNotReceive().Start();

            Assert.AreEqual(2, _numOfCompletedEvents);
        }
		#endregion ITask.Start()

		#region ITask.Stop()
        [Test]
        public void Stop_WhileNotStarted_DoesNothing()
        {
            _taskController.Stop();
            _task.DidNotReceive().Stop();
        }

		[Test]
		public void Stop_WhileStarted_StopsTask()
		{
            _taskController.Start();
			_taskController.Stop();
            _task.Received().Stop();
		}

		[Test]
		public void Stop_WhileResumed_StopsTask()
		{
            _taskController.Start();    // Start
            _taskController.Stop();     // Stop
            _taskController.Start();    // Resume

            _task.ClearReceivedCalls();
			_taskController.Stop();
			_task.Received().Stop();
		}

		[Test]
		public void Stop_WhileStopped_DoesNothing()
		{
            Stop_WhileStarted_StopsTask();

			_task.ClearReceivedCalls();
			_taskController.Stop();
            _task.DidNotReceive().Stop();
		}

		[Test]
        public void Stop_WhileCompleted_DoesNothing()
		{
			_taskController.Start();
			_task.Completed += Raise.Event();

            _taskController.Stop();
			_task.DidNotReceive().Stop();
		}
		#endregion ITask.Stop()

		private void _task_Completed(object sender, EventArgs e)
		{
			_numOfCompletedEvents++;
		}
    }
}
