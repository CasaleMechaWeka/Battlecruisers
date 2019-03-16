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
    public class PrioritisedTask : IPrioritisedTask, ICompletedEventEmitter
    {
        private readonly ITask _task;
        private IState _currentState;

        public TaskPriority Priority { get; }

        public event EventHandler<EventArgs> Completed;

        public PrioritisedTask(TaskPriority priority, ITask task)
        {
            Priority = priority;
            _task = task;
            _currentState = new InitialState(_task, this);

            _task.Completed += _task_Completed;
        }

        public void Start()
        {
            _currentState = _currentState.Start();
        }

        public void Stop()
        {
            _currentState = _currentState.Stop();
        }
        
        public void EmitCompletedEvent()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }
		
		private void _task_Completed(object sender, EventArgs e)
		{
			_task.Completed -= _task_Completed;
			_currentState = _currentState.OnCompleted();
		}

        public override string ToString()
        {
            return _task.ToString();
        }
    }
}
