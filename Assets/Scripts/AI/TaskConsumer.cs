using System;

namespace BattleCruisers.AI
{
    /// <summary>
    /// Starts the highest priority task.  Once completed,
    /// starts the next highest priority task.  If the highest
    /// priority task changes, stops the current task and starts
    /// the new highest priority task.
    /// </summary>
    public class TaskConsumer : ITaskConsumer
    {
        private readonly ITaskList _tasks;

        private ITask _currentTask;
        private ITask CurrentTask
        {
            get { return _currentTask; }
            set
            {
                if (_currentTask != null)
                {
                    _currentTask.Stop();
                    _currentTask.Completed -= _currentTask_Completed;
                }

                _currentTask = value;

                if (_currentTask != null)
                {
                    _currentTask.Completed += _currentTask_Completed;
                    _currentTask.Start();
                }
            }
        }

        public TaskConsumer(ITaskList tasks)
        {
            _tasks = tasks;
            CurrentTask = _tasks.HighestPriorityTask;
            _tasks.HighestPriorityTaskChanged += _tasks_HighestPriorityTaskChanged;
        }

        private void _currentTask_Completed(object sender, EventArgs e)
        {
            _tasks.Remove(CurrentTask);
            CurrentTask = _tasks.HighestPriorityTask;
        }

        private void _tasks_HighestPriorityTaskChanged(object sender, EventArgs e)
        {
            CurrentTask = _tasks.HighestPriorityTask;
        }
    }
}
