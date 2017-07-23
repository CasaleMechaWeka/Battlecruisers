using System;

namespace BattleCruisers.AI.Tasks
{
    public enum TaskPriority
	{
		Normal, High
	}

    public interface ITask
    {
        TaskPriority Priority { get; }

        event EventHandler<EventArgs> Completed;

        // FELIX  If started when already completed, should emit Completed event.
        void Start();

        // FELIX  Should have no effect if already completed (ie, no crash :P )
        void Stop();
    }
}
