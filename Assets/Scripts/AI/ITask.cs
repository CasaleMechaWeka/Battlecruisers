using System;

namespace BattleCruisers.AI
{
    public enum TaskPriority
	{
		Normal, High
	}

    public interface ITask
    {
        TaskPriority Priority { get; }

        event EventHandler<EventArgs> Completed;

        // FELIX If started when already completed, should emit Completed event.
        void Start();
        void Stop();
    }
}
