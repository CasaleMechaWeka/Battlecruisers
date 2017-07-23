using System;

namespace BattleCruisers.AI.Tasks
{
    public abstract class Task : ITask
    {
        protected bool _isCompleted;

        public TaskPriority Priority { get; private set; }

        public event EventHandler<EventArgs> Completed;

        public Task(TaskPriority priority)
        {
            Priority = priority;
            _isCompleted = false;
        }

        public virtual void Start()
        {
            if (_isCompleted)
            {
                EmitCompletedEvent();
            }
        }

        public virtual void Stop()
        {
            
        }

        protected void EmitCompletedEvent()
        {
            if (Completed != null)
            {
                Completed.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
