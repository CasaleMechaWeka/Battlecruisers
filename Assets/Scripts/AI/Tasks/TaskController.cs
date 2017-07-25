using System;
using BattleCruisers.AI.Tasks.States;

namespace BattleCruisers.AI.Tasks
{
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
    public class TaskController : ITask, ICompletedEventEmitter
    {
        private readonly IInternalTask _task;
        private IState _currentState;

        public TaskPriority Priority { get; private set; }

        public event EventHandler<EventArgs> Completed;

        public TaskController(TaskPriority priority, IInternalTask task)
        {
            Priority = priority;
            _task = task;
            _currentState = new InitialState(_task, this);

            _task.Completed += _task_Completed;
        }

        public virtual void Start()
        {
            _currentState = _currentState.Start();
        }

        public virtual void Stop()
        {
            _currentState = _currentState.Stop();
        }
        
        public void EmitCompletedEvent()
        {
            if (Completed != null)
            {
                Completed.Invoke(this, EventArgs.Empty);
            }
        }
		
		private void _task_Completed(object sender, EventArgs e)
		{
			_task.Completed -= _task_Completed;
			_currentState = _currentState.OnCompleted();
		}
    }
}
