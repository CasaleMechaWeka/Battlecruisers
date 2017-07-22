using System;

namespace BattleCruisers.AI
{
    public enum TaskPriority
    {
        Normal, High
    }

    public class NewTaskEventArgs : EventArgs
    {
        public TaskPriority Priority { get; private set; }

        public NewTaskEventArgs(TaskPriority priority)
        {
            Priority = priority;
        }
    }

    public interface ITaskProducer
    {
        bool HasTasks();
        ITask NextTask();
        event EventHandler<NewTaskEventArgs> NewTaskProduced;
    }
}
