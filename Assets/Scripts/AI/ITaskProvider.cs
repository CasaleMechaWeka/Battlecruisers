using System;

namespace BattleCruisers.AI
{
    public class NewTaskEventArgs : EventArgs
    {
        public TaskPriority Priority { get; private set; }

        public NewTaskEventArgs(TaskPriority priority)
        {
            Priority = priority;
        }
    }

    public interface ITaskProvider
    {
        bool HasTasks { get; }

        event EventHandler<NewTaskEventArgs> NewTaskProduced;
		
        ITask NextTask();
    }
}
