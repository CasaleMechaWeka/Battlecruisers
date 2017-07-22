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

        void Start();
        void Stop();
    }
}
