using System;
using UnityEngine.Assertions;

namespace BattleCruisers.AI
{
    // FELIX  Use states to avoid ifs :)
    public class TaskConsumer : ITaskConsumer
    {
        private readonly ITaskProvider _normalPriorityProvider, _highPriorityProvider;

        private ITask _currentNormalPriorityTask, _currentHighPriorityTask;

        public TaskConsumer(ITaskProvider normalPriorityProvider, ITaskProvider highPriorityProvider)
        {
            _normalPriorityProvider = normalPriorityProvider;
            _highPriorityProvider = highPriorityProvider;
        }

        public void Start()
        {
            _normalPriorityProvider.NewTaskProduced += _normalPriorityProvider_NewTaskProduced;
            _highPriorityProvider.NewTaskProduced += _highPriorityProvider_NewTaskProduced;
        }

        // Start if no normal or high priority tasks
        private void _normalPriorityProvider_NewTaskProduced(object sender, NewTaskEventArgs e)
        {
            if (_currentHighPriorityTask == null
                && _currentNormalPriorityTask == null)
            {
                Assert.IsTrue(_normalPriorityProvider.HasTasks);
                StartNextNormalPriorityTask();
            }
        }

        // Start next normal task if no higher priority tasks
        private void _currentNormalPriorityTask_Completed(object sender, EventArgs e)
        {
            _currentNormalPriorityTask.Completed -= _currentNormalPriorityTask_Completed;
            _currentNormalPriorityTask = null;

            if (_currentHighPriorityTask == null
                && _normalPriorityProvider.HasTasks)
            {
                StartNextNormalPriorityTask();
            }
        }
		
		private void StartNextNormalPriorityTask()
		{
			_currentNormalPriorityTask = _normalPriorityProvider.NextTask();
			_currentNormalPriorityTask.Completed += _currentNormalPriorityTask_Completed;
			_currentNormalPriorityTask.Start();
		}

        // Start if no high priority tasks, pause any normal priority task
        private void _highPriorityProvider_NewTaskProduced(object sender, NewTaskEventArgs e)
        {
            if (_currentHighPriorityTask == null)
            {
                if (_currentNormalPriorityTask != null)
                {
                    _currentNormalPriorityTask.Pause();
                }

                Assert.IsTrue(_highPriorityProvider.HasTasks);
                StartNextHighPriorityTask();
            }
        }

        // Start next high priority task.  Resume existing normal priority, or next normal priority task.
        private void _currentHighPriorityTask_Completed(object sender, EventArgs e)
        {
            _currentHighPriorityTask.Completed -= _currentHighPriorityTask_Completed;
            _currentHighPriorityTask = null;

            if (_highPriorityProvider.HasTasks)
            {
                StartNextHighPriorityTask();
            }
            else if (_currentNormalPriorityTask != null)
            {
                _currentNormalPriorityTask.Resume();
            }
            else if (_normalPriorityProvider.HasTasks)
            {
                StartNextNormalPriorityTask();
            }
        }
		
        private void StartNextHighPriorityTask()
        {
            _currentHighPriorityTask = _highPriorityProvider.NextTask();
            _currentHighPriorityTask.Completed += _currentHighPriorityTask_Completed;
            _currentHighPriorityTask.Start();
        }
    }
}
