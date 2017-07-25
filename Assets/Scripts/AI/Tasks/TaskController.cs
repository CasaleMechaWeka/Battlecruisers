using System;

namespace BattleCruisers.AI.Tasks
{
	// FELIX  use states instead of branching:  Initial, InProgress, Stopped, Completed

	// Converts:
	/// <summary>
	/// Converts:
	/// + ITask.Start()
	/// + ITask.Stop()
	/// 
	/// Into:
	/// + IInternalTask.Start() => Only ever called once
	/// + IInternalTask.Stop()  => Only called if started or resumed
	/// + IInternalTask.Resume()=> Only called if stopped
	/// </summary>
	public class TaskController : ITask
    {
        private readonly IInternalTask _task;

        protected bool _isCompleted, _isStopped, _isStarted;

        public TaskPriority Priority { get; private set; }

        public event EventHandler<EventArgs> Completed;

        public TaskController(TaskPriority priority, IInternalTask task)
        {
            Priority = priority;
            _task = task;
            _isCompleted = false;
            _isStopped = false;
            _isStarted = false;

            _task.Completed += _task_Completed;
        }

        public virtual void Start()
        {

            if (_isCompleted)
            {
                EmitCompletedEvent();
            }
            else if (_isStopped)
            {
                _task.Resume();
                _isStopped = false;
            }
            else if (!_isStarted)
            {
                _task.Start();
				_isStarted = true;
            }
        }

        public virtual void Stop()
        {
            if (_isStarted && !_isStopped && !_isCompleted)
            {
                _task.Stop();
                _isStopped = true;
            }
        }

        private void _task_Completed(object sender, EventArgs e)
        {
            _task.Completed -= _task_Completed;
            _isCompleted = true;
            EmitCompletedEvent();
        }
		
		private void EmitCompletedEvent()
		{
			if (Completed != null)
			{
				Completed.Invoke(this, EventArgs.Empty);
			}
		}
    }
}
