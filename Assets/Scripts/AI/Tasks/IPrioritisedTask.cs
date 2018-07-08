using System;

namespace BattleCruisers.AI.Tasks
{
    public enum TaskPriority
	{
		Normal, High
	}

    public interface IPrioritisedTask
    {
        TaskPriority Priority { get; }

        event EventHandler<EventArgs> Completed;

        // Immediately emits Completed event if task is already completed.
        void Start();
        void Stop();
    }
}
