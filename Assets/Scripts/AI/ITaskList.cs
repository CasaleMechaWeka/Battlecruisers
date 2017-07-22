using System;

namespace BattleCruisers.AI
{
    public interface ITaskList
    {
        bool IsEmpty { get; }

        event EventHandler HighestPriorityTaskChanged;

        void Add(ITask taskToRemove);
        void Remove(ITask taskToAdd);
        ITask GetHighestPriorityTask();
    }
}
